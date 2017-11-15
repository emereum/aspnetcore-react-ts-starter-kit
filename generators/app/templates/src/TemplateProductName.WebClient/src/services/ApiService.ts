import * as ApiConstants from "./ApiConstants";
import * as moment from "moment";

// So we can distinguish fetch requests on the backend
const headers = new Headers();
headers.append("X-Requested-With", "XMLHttpRequest");

const defaultFetchOptions: RequestInit = {
    credentials: "include", // Allow cookies to be sent and received cross-origin
    headers: headers
};

class ApiService {
    get(url: string, data?: object) {
        // Maybe add data to the url string as encoded parameters
        if(data != null) {
            url += "?" + this.parameterize(data);
        }

        return fetch(ApiConstants.baseUrl + url, defaultFetchOptions)
            .then(this.maybeParseJson);
    }

    post(url: string, data?: object) {
        return fetch(ApiConstants.baseUrl + url, {
            ...defaultFetchOptions,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json",
                "X-Requested-With": "XMLHttpRequest"
            }),
            body: data != null ? JSON.stringify(data) : null
        })
        .then(this.maybeParseJson);
    }

    /**
     * Deserialize the response as JSON if it has a JSON content-type, otherwise return null.
     * @param response 
     */
    private maybeParseJson(response: Response) {
        const contentType = response.headers.get("content-type");
        return (contentType != null && contentType.indexOf("application/json") > -1)
            ? response.json()
            : null;
    }

    /**
     * Converts an object to a URL query string like ?thing=blah&anotherThing=Yep
     * See: http://stackoverflow.com/questions/22678346/convert-javascript-object-to-url-parameters
     */ 
    private parameterize(data: object) {
        return Object
            .keys(data)
            .map(k => encodeURIComponent(k) + "=" + this.encode(data[k]))
            .join("&");
    }

    private encode(value: any) {
        if (value == null) {
            return "";
        } else if (value instanceof moment) {
            return encodeURIComponent(value.format());
        } else {
            return encodeURIComponent(value);
        }
    }
    
}

export default new ApiService();