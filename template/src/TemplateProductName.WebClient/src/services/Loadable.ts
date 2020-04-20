import { action, computed, observable } from "mobx";

type LoadStatus = "initial" | "loading" | "loaded" | "networkError";

/**
 * Loadable represents some data that can be requested from the Api and the
 * various states of loading it goes through. This can be used to orchestrate
 * the UI when loading a resource. For example, it can be used to show a loading
 * spinner when an Api request begins.
 * 
 * Loadable integrates with the Api class to provide a simple means to handle an
 * Api request.
 */
export default class Loadable<TResponse> {
    @observable status: LoadStatus = "initial";
    @observable data?: TResponse;
    @computed get isLoading() { return this.status === "loading"; }
    @computed get isLoaded() { return this.status === "loaded"; }
    @computed get hasNetworkError() { return this.status === "networkError"; }

    @action loading = () => {
        this.status = "loading";
        this.data = undefined;
    };

    @action loaded = (data?: TResponse) => {
        this.status = "loaded";
        this.data = data;
    };

    @action networkError =() => {
        this.status = "networkError";
        this.data = undefined;
    };
}