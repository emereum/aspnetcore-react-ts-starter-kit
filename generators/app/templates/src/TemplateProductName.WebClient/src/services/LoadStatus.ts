import { observable } from "mobx";

export default class LoadStatus {
    @observable isLoading: boolean;
    @observable networkError: boolean;
}