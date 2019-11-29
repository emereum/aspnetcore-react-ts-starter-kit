import { observable } from "mobx";
import Api from "../../services/Api";
import { toast } from "react-toastify";
import * as H from "history";
import AppStore from "../app/AppStore";

export default class SignInStore {
    @observable isSigningIn: boolean = false;
    @observable email: string = "";
    @observable password: string = "";
    history: H.History;

    constructor(history: H.History) {
        this.history = history;
    }

    signIn = () => {
        this.isSigningIn = true;
        Api
            .post("/authentication/signin", { email: this.email, password: this.password})
            .then((response) => {
                if(response.data != null) {
                    toast.error("The username or password is invalid!");
                    return undefined;
                }
                
                // We are logged in so get the user details
                return AppStore.loadUser();
            })
            .then((user) => {
                if(user != null) {
                    this.history.push("/");
                } else {
                    toast.error("Something went wrong. Please try again.")
                }
                this.isSigningIn = false;
            })
            .catch(() => {
                toast.error("Something went wrong. Please try again.");
                this.isSigningIn = false;
            })
    }
}