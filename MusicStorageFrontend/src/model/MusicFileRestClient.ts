import axios from "axios";
import { config } from "../config";
import { AppStore, ClassStateField, StateModule } from "../store/AppStore";
import IFilterMusicFile from "./IFilterMusicFile";
import { IMusicFile } from "./IMusicFile";

export abstract class MusicFileRestClient {
  @ClassStateField(StateModule.GENERAL)
  public static searchMusicFile: string;

  @ClassStateField(StateModule.GENERAL)
  public static sortByMusicFile: string;

  public static async getMusicFiles(): Promise<any> {
    const filter: IFilterMusicFile = AppStore.state.filterMusicFile;
    const searchMusicFile = this.checkEmpty(this.searchMusicFile);
    const sortByMusicFile = this.checkEmpty(this.sortByMusicFile);
    const genre = this.checkEmpty(filter.genre);
    const interpret = this.checkEmpty(filter.interpret);

    return await axios.get(
      `${this.api}/musicfile/sortBy=${sortByMusicFile}/search=${searchMusicFile}/filter/interpret=${interpret}/genre=${genre}`
    );
  }

  public static async getInterprets(): Promise<any> {
    return await axios.get(`${this.api}/interpret/sortBy=name/search=null`);
  }

  public static createMusicFile(musicFile: IMusicFile): Promise<any> {
    return axios.post(`${this.api}/musicfile`, { musicFile });
  }

  public static updateMusicFile(musicFile: IMusicFile): Promise<any> {
    const id = musicFile.ID;
    return axios.put(`${this.api}/musicfile/${id}`, { musicFile });
  }

  public static deleteMusicFile(id: number): Promise<any> {
    return axios.delete(`${this.api}/musicfile/${id}`);
  }

  public static async getInterpret(id: number): Promise<any> {
    return await axios.get(`${this.api}/interpret/${id}`);
  }

  public static deleteInterpret(id: number): Promise<any> {
    return axios.delete(`${this.api}/interpret/${id}`);
  }

  public static checkEmpty(txt: any) {
    if (txt === "" || txt === undefined) { return null; } else { return txt; }
  }
  private static api = config.apiEndpoint;
}
