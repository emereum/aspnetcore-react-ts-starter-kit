import { observer } from "mobx-react";
import React from "react";
import SignInStore from "./SignInStore";
import { RouteComponentProps } from "react-router";

@observer
export default class SignInComponent extends React.Component<RouteComponentProps<{}>, {}> {
    store: SignInStore;

    constructor(props: RouteComponentProps<{}>) {
        super(props);
        this.store = new SignInStore(props.history);
    }
    
    render() {
        const { isSigningIn, email, password, signIn} = this.store;

        return(
            <div>
                <form action="" className="box" onClick={(e) => e.preventDefault()}>
                    <div className="field">
                    <label className="label">Email</label>
                    <div className="control has-icons-left">
                        <input type="email" placeholder="e.g. bobsmith@gmail.com" className="input" value={email} onChange={(e) => this.store.email = e.target.value} required />
                        <span className="icon is-small is-left">
                        <i className="fa fa-envelope"></i>
                        </span>
                    </div>
                    </div>
                    <div className="field">
                    <label className="label">Password</label>
                    <div className="control has-icons-left">
                        <input type="password" placeholder="*******" className="input" value={password} onChange={(e) => this.store.password = e.target.value} required />
                        <span className="icon is-small is-left">
                        <i className="fa fa-lock"></i>
                        </span>
                    </div>
                    </div>
                    <div className="field">
                    </div>
                    <div className="field">
                    <button className={"button is-primary" + (isSigningIn ? " is-loading" : "")} onClick={signIn}>
                        Login
                    </button>
                    </div>
                </form>
            </div>
        );
    }
}