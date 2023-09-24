export interface IChatLoginRequestModel{
    Username: string;
    Age: number;
    City: string;
    Gender: string;
}
export class ChatLoginRequestModel implements IChatLoginRequestModel{
    Username: string = "";
    Age: number = 0;
    City: string = "";
    Gender: string = "";
}