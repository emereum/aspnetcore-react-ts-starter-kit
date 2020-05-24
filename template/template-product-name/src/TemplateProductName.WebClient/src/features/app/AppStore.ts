import { observable, computed } from "mobx";
import { toast } from "react-toastify";
import { UserModel, AuthenticationApi } from "../../services/TemplateProductNameApi";


class AppStore {
    @observable user?: UserModel;
    userPromise: Promise<UserModel | undefined> = Promise.resolve(undefined);
    @observable isSigningOut: boolean = false;
    @computed get isSignedIn() {
        return this.user != null;
    }
    private authenticationApi = new AuthenticationApi();

    loadUser = () => 
        this.userPromise = this.authenticationApi
            .me()
            .then(result => {
                this.user = result.data;
                return result.data
            });
    

    signOut = () => {
        this.isSigningOut = true;
        this.authenticationApi
            .signOut()
            .then(response => {
                if(!response.ok) {
                    toast.error("Something went wrong. Please try again.")
                }
                // Success
                // todo: Use the other History api rather than this.
                window.location.assign("/");
                this.isSigningOut = false;
            })
            .catch(() => {
                toast.error("Something went wrong. Please try again.")
                this.isSigningOut = false;
            });
    }

    constructor() {
        // This will be called when the SPA is first loaded. Let's see if we're
        // logged in. Only check if we have a login cookie set to avoid pounding
        // our API with loads of requests coming from users who are definitely
        // not signed in.
        if(document.cookie.indexOf("IsSignedIn") > -1) {
            // todo: Should we put a loading spinner over the app while we load?
            // Otherwise the user will see the page in a logged-out context on
            // first load.
            this.loadUser();
        }
    }
}

export default new AppStore();