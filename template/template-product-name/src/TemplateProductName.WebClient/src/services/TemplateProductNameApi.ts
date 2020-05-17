﻿/* tslint:disable */
/* eslint-disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.5.0.0 (NJsonSchema v10.1.15.0 (Newtonsoft.Json v12.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming

import Api from "./Api";

export class AuthenticationApi {

    signIn = (command: SignInCommand) =>
        Api.post<SignInCommand, IErrors>("/Authentication/signin", command);

    signOut = () =>
        Api.post<object, void>("/Authentication/signout");

    me = () =>
        Api.get<object, UserModel>("/Authentication/me");
}

export class DinosaursApi {

    post = (command: CreateDinosaurCommand) =>
        Api.post<CreateDinosaurCommand, IErrors>("/Dinosaurs", command);
}

export class HomeApi {

    index = () =>
        Api.get<object, FileResponse>("/Home");
}

export interface IErrors {
}

export interface SignInCommand {
    email?: string | undefined;
    password?: string | undefined;
}

export interface UserModel {
    id?: string;
    email?: string | undefined;
}

export interface CreateDinosaurCommand {
    id?: string;
    name?: string | undefined;
}

export interface FileResponse {
    data: Blob;
    status: number;
    fileName?: string;
    headers?: { [name: string]: any };
}

export class ApiException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): any {
    if (result !== null && result !== undefined)
        throw result;
    else
        throw new ApiException(message, status, response, headers, null);
}