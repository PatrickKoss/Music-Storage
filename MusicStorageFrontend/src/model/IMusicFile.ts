import ITitle from "./ITitle";
import IInterpret from "./IInterpret";

export interface IMusicFile {
    ID: Number;
    Genre: String;
    Title: ITitle;
    Interpret: IInterpret;
}