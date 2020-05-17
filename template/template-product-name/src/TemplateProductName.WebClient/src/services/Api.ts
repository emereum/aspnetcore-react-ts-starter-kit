
import * as moment from "moment";
import ValidationErrors from "../common/ValidationErrors";
import * as ApiConstants from "./ApiConstants";
import ApiResponse from "./ApiResponse";
import Loadable from "./Loadable";
import * as Toasts from "../common/Toasts";

const defaultFetchOptions: RequestInit = {
    credentials: "include", // Allow cookies to be sent and received cross-origin
    headers: { "X-Requested-With": "XMLHttpRequest" } // So we can distinguish fetch requests on the backend
};

/**
 * Provides methods to execute commands or queries against the Api. Assumes
 * commands will conventionally return either an OK response or a list of
 * errors.
 */
class Api {
    /**
     * Executes a query against the Api and returns the results of the query.
     * This should never be used to execute commands or perform any operation
     * that changes the state of the backend. It should only be used for
     * retrieving information. 
     */
    get<TQuery extends object, TResponse>(url: string, data?: TQuery, loadable?: Loadable<TResponse>) {
        if(loadable != null) {
            loadable.loading();
        }

        // Maybe add data to the url string as encoded parameters
        if(data != null) {
            url += "?" + this.parameterize(data);
        }

        // Invoke the http request
        return fetch(ApiConstants.baseUrl + url, defaultFetchOptions)

            // Attempt to parse the JSON response if it's a JSON response
            .then(this.maybeParseJson)

            // Update the loadable status (if we loaded the data)
            .then((x: ApiResponse<TResponse>) => {
                if(loadable != null) {
                    loadable.loaded(x.data);
                }
                return x;
            })

            // Make a network error toast and update the loadable status (if we had
            // a network error)
            .catch((reason: any) => {
                Toasts.networkError();            
                if(loadable != null) {
                    loadable.networkError();
                }
                throw reason;
            });
    }

    /**
     * Posts a command to the Api and returns a status code and optionally
     * some response data which should be a set of validation errors. This
     * should typically be used when invoking a command.
     * 
     * The TResponse generic parameter should only be specified in rare
     * circumstances when a command is expected to return something other than
     * an OK response or a list of errors.
     */
    post<TCommand, TResponse = ValidationErrors>(url: string, data?: TCommand, loadable?: Loadable<TResponse>) {
        if(loadable != null) {
            loadable.loading();
        }

        // Invoke the http request
        return fetch(ApiConstants.baseUrl + url, {
                ...defaultFetchOptions,
                method: "post",
                headers: {
                    ...defaultFetchOptions.headers,
                    "Content-Type": "application/json"
                },
                body: data != null ? JSON.stringify(data) : null
            })

            // Attempt to parse the JSON response if it's a JSON response
            .then(this.maybeParseJson)

            // Help the type system a bit...
            .then(x => x as ApiResponse<TResponse>)

            // Update the loadable status (if we loaded the data)
            .then((x: ApiResponse<TResponse>) => {
                if(loadable != null) {
                    loadable.loaded(x.data);
                }
                return x;
            })

            // Make a network error toast and update the loadable status (if we
            // had a network error)
            .catch((reason: any) => {
                Toasts.networkError();
                if(loadable != null) {
                    loadable.networkError();
                }
                throw reason;
            });
    }

    /**
     * Deserialize the response data as JSON if it has a JSON content-type and
     * add it to the Response object, otherwise return the Response object
     * as-is.
     * @param response 
     */
    private maybeParseJson(response: Response) {
        if((response.headers.get("Content-Type") || "").indexOf("application/json") === -1) {
            return Promise.resolve(response);
        }

        return response
            .json()
            .then(data => {
                (response as any).data = data;
                return response;
            });
    }

    /**
     * Converts an object to a URL query string like ?thing=blah&anotherThing=Yep
     * See: http://stackoverflow.com/questions/22678346/convert-javascript-object-to-url-parameters
     */ 
    private parameterize(data: {[key:string]: any}) {
        return Object
            .keys(data)
            .map(k => encodeURIComponent(k) + "=" + this.encode(data[k]))
            .join("&");
    }

    private encode(value: any) {
        if (value == null) {
            return "";
        } else if (moment.isMoment(value)) {
            return encodeURIComponent(value.format());
        } else {
            return encodeURIComponent(value);
        }
    }
    
}

export default new Api();