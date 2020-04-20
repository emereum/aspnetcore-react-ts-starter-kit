import { observable } from "mobx";
import * as uuid from "uuid";

export default class Dinosaur {
    @observable id: string;
    @observable name: string;

    constructor() {
        this.id = uuid.v4();
        this.name = "";
    }
}