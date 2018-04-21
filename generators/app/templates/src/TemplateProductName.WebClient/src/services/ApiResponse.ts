export default class ApiResponse<TResponse> extends Response {
    /**
     * Deserialised json response data (or undefined)
     */
    data?: TResponse;
}