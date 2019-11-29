class Constants {
    readonly isDebug: boolean;

    constructor() {
        const isLocalhost = window.location.hostname.indexOf("localhost") > -1;

        if(isLocalhost) {
            this.isDebug = true;
        } else {
            this.isDebug = false;
        }
    }
}

export default new Constants();