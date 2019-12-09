import axios from "axios";
import { config } from "../config";

export abstract class UserRestClient {
    public static async getUsers(): Promise<any> {
        return await axios.get(`${this.api}/Persons`);
    }

    public static async getInterpret(): Promise<any> {
        let search = null;
        let sortBy = null;
        return await axios.get(`${this.api}/Interpret/${search}/${sortBy}`);
    }

    private static api = config.apiEndpoint;
}