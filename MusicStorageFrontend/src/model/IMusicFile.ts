import IInterpret from "./IInterpret";
import ITitle from "./ITitle";

export interface IMusicFile {
    ID: Number;
    Genre: String;
    Title: ITitle;
    Interpret: IInterpret;
}
