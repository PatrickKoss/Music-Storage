import Vue from "vue";
import Vuex from "vuex";
import { GeneralState } from "./states/GeneralState";
import IFilterMusicFile from "../model/IFilterMusicFile";
import { MusicFileRestClient } from "../model/MusicFileRestClient";
import { IMusicFile } from "../model/IMusicFile";
import { IMusicFileWithoutIDs } from "../model/IMusicFileWithoutIDs";
import IInterpret from "../model/IInterpret";

Vue.use(Vuex);

/**
 * All the namespaces for the modules
 */
export const enum StateModule {
    GENERAL = "GENERAL",
    SYSTEM = "SYSTEM",
    SCENARIO = "SCENARIO"
}

export const AppStore = new Vuex.Store({
    /**
     * Register a new module here
     *
     * MYMODULE : new MyModule().state
     */
    modules: {
        GENERAL: new GeneralState().state,
    },

    /**
     * since you can only put primitive variables into the vuex and do get() and set() methods you have to put all
     * other variables into the state attribute
     */
    state: {
        filterMusicFile: {
            genre: "",
            interpret: "",
        } as IFilterMusicFile,
        musicFiles: Array<IMusicFile>(),
        musicFilesWithoutIDs: Array<IMusicFileWithoutIDs>(),
        loadingMusicFiles: false,
        interprets: Array<String>(),
    },

    /**
     * if you want to mutate state variables you have to write methods that set the variables
     */
    mutations: {
        setFilterMusicFile(state, filter: IFilterMusicFile) {
            state.filterMusicFile = filter;
        },
        async loadMusicFiles(state) {
            state.loadingMusicFiles = true;
            let data: Array<IMusicFile> = (await MusicFileRestClient.getMusicFiles())
            .data;
            state.musicFiles = data;
            state.musicFilesWithoutIDs = Array<IMusicFileWithoutIDs>();
            data.forEach((musicFile) => {
                state.musicFilesWithoutIDs.push({
                    ID: musicFile.ID,
                    Title: musicFile.Title.Name,
                    Interpret: musicFile.Interpret.Name,
                    Genre: musicFile.Genre
                } as IMusicFileWithoutIDs);
            });
            state.loadingMusicFiles = false;
        },
        async loadInterprets(state) {
            let data: Array<IInterpret> = (await MusicFileRestClient.getInterprets())
            .data;
            state.interprets = Array<string>()
            data.forEach(interpret => {
                state.interprets.push(interpret.Name);
            });
        },
        resetFilterMusicFile(state) {
            state.filterMusicFile = {
                genre: "",
                interpret: "",
            } as IFilterMusicFile
        },
    }
});

/**
 * @decorator
 *
 * With this decorator you can map any property in a normal class to a stored value in the Store.
 * If you call the get/set the property the get/set function is going to be replaced with getters/setters
 * stored in the DecoratorFactory.
 *
 * Has to stay in this file to avoid circular dependencies. (needs AppStore instance)
 *
 * @param namespace - namespace representing a StateModule
 * @param fieldName - a override for the key if the variable is local key is called different
 */
export function ClassStateField(namespace: StateModule, fieldName?: string) {
    return (target: any, key: string | symbol) => {
        const keyName = fieldName
            ? namespace.toString() + "/" + fieldName.toString()
            : namespace.toString() + "/" + key.toString();

        const getter = () => {
            return AppStore.getters[keyName];
        };

        const setter = value => {
            AppStore.commit(keyName, value);
        };

        return {
            get: getter,
            set: setter
        } as any;
    };
}
