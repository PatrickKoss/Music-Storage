import { State, StateHandler } from "../State";

/**
 * this class puts general variables into the Vuex store
 */
@StateHandler()
export class GeneralState extends State {
    public static darkeningGeneral: boolean = false;
    public static strings: any = {};
    public static error: string = "";
    public static showError: boolean = false;
    public static timeout: number = 10000;
    public static disableElements: boolean = false;
    public static design: number = 1;
    public static loadData: boolean = false;
    public static errorBool: boolean = false;
    public static searchMusicFile: string = "";
    public static sortByMusicFile: string = "title";
    public static sortByInterpret: string = "name";
    public static searchInterpret: string = "";
}
