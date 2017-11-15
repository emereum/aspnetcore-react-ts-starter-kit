import { observable } from "mobx";

export default class Dinosaur {
    @observable name: string;

    constructor() {
        this.name = "";
    }
}