
import * as moment from "moment";
import * as ApiConstants from "./ApiConstants";
import Loadable from "./Loadable";
import ValidationErrors from "../common/ValidationErrors";

const defaultFetchOptions: RequestInit = {
    credentials: "include", // Allow cookies to be sent and received cross-origin
    headers: { "X-Requested-With": "XMLHttpRequest" } // So we can distinguish fetch requests on the backend
};

class ApiResponse<TResponse> extends Response {
    /**
     * Deserialised json response data (or undefined)
     */
    data?: TResponse;
}

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
    get<TQuery extends Object, TResponse>(url: string, data?: TQuery, loadable?: Loadable<TResponse>) {
        if(loadable != null) {
            loadable.loading();
        }

        // Maybe add data to the url string as encoded parameters
        if(data != null) {
            url += "?" + this.parameterize(data);
        }

        const promise =
            // Invoke the http request
            fetch(ApiConstants.baseUrl + url, defaultFetchOptions)

            // Attempt to parse the JSON response if it's a JSON response
            .then(this.maybeParseJson)

            // Help the type system a bit...
            .then(x => <ApiResponse<TResponse>>x)

            // Update the loadable status (if we loaded the data)
            .then((x: ApiResponse<TResponse>) => {
                if(loadable != null) {
                    loadable.loaded(x.data);
                }
                return x;
            });

            // Update the loadable status (if we had a network error)
        promise.catch((reason: any) => {
            if(loadable != null) {
                loadable.networkError();
            }
            return reason;
        });
        
        return promise;
    }

    /**
     * Posts a command to the Api and returns a status code and optionally
     * a list of errors. This should typically be used when invoking a command.
     */
    post<TCommand>(url: string, data?: TCommand, loadable?: Loadable<ValidationErrors>) {
        return this.postBase<TCommand, ValidationErrors>(url, data, loadable);
    }

    /**
     * Posts a command to the Api and returns an object of type TResponse. This
     * should only be used in rare circumstances when a command is expected to
     * return something other than an OK response or a list of errors. 
     */
    postBase<TCommand, TResponse>(url: string, data?: TCommand, loadable?: Loadable<TResponse>) {
        if(loadable != null) {
            loadable.loading();
        }

        const promise =
            // Invoke the http request
            fetch(ApiConstants.baseUrl + url, {
                ...defaultFetchOptions,
                method: "post",
                headers: {
                    ...defaultFetchOptions.headers,
                    "Content-Type": "application/json"
                },
                body: data != null ? JSON.stringify(data) : null
            })

            // Attempt to parse the JSON response if it's a JSON response
            .then(x => this.maybeParseJson(x))

            // Help the type system a bit...
            .then(x => <ApiResponse<TResponse>>x)

            // Update the loadable status (if we loaded the data)
            .then((x: ApiResponse<TResponse>) => {
                if(loadable != null) {
                    loadable.loaded(x.data);
                }
                return x;
            });

        // Update the loadable status (if we had a network error)
        promise.catch((reason: any) => {
            if(loadable != null) {
                loadable.networkError();
            }
        });

        return promise;
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
                (<any>response).data = data;
                return response;
            });
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

export default new Api();