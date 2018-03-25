import { observable } from "mobx";
import * as uuid from "uuid";

export default class Dinosaur {
    @observable guid: string;
    @observable name: string;

    constructor() {
        this.guid = uuid.v4();
        this.name = "";
    }
}