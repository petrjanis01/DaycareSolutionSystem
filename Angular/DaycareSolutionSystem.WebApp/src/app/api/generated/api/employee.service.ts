/* tslint:disable */
/**
 * My API
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * OpenAPI spec version: v1
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */

import { Injectable, Optional } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

import { ApiBase } from 'src/app/api/api-base';
import { NotifiactionService } from '../../../services/notification.service';
import { Router } from '@angular/router';

import { CustomHttpUrlEncodingCodec } from '../encoder';

import { EmployeeBasicDTO } from '../model/employeeBasicDTO';
import { EmployeeDetailDTO } from '../model/employeeDetailDTO';
import { PictureDTO } from '../model/pictureDTO';

import { Configuration } from '../configuration';


@Injectable({
  providedIn: 'root'
})
export class EmployeeService extends ApiBase{
    public defaultHeaders = new HttpHeaders();
    public configuration = new Configuration();

    constructor(
        private httpClient: HttpClient,
        router: Router,
        notification: NotifiactionService,
        @Optional() configuration: Configuration) {
        super(router, notification);
    }

    /**
     * @param consumes string[] mime-types
     * @return true: consumes contains 'multipart/form-data', false: otherwise
     */
    private canConsumeForm(consumes: string[]): boolean {
        const form = 'multipart/form-data';
        for (const consume of consumes) {
            if (form === consume) {
                return true;
            }
        }
        return false;
    }

    public async apiEmployeeAllCaregiversGet(): Promise<Array<EmployeeBasicDTO>> {

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];


        let result = this.httpClient.get<Array<EmployeeBasicDTO>>(`${this.basePath}/api/Employee/all-caregivers`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: this.createAuthHeaders(headers),
            }
        ).toPromise();

        return this.processErrors(result);
    }

    public async apiEmployeeChangePasswordPut(newPassword?: string, ): Promise<any> {


        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (newPassword !== undefined && newPassword !== null) {
            queryParameters = queryParameters.set('newPassword', <any>newPassword);
        }

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];


        let result = this.httpClient.put<any>(`${this.basePath}/api/Employee/change-password`,
            null,
            {
                params: queryParameters,
                withCredentials: this.configuration.withCredentials,
                headers: this.createAuthHeaders(headers),
            }
        ).toPromise();

        return this.processErrors(result);
    }

    public async apiEmployeeChangeProfilePicturePost(employeeId?: string, PictureDTO?: PictureDTO, ): Promise<any> {



        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (employeeId !== undefined && employeeId !== null) {
            queryParameters = queryParameters.set('employeeId', <any>employeeId);
        }

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
            'application/json',
            'text/json',
            'application/_*+json'
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }


        let result = this.httpClient.post<any>(`${this.basePath}/api/Employee/change-profile-picture`,
            PictureDTO,
            {
                params: queryParameters,
                withCredentials: this.configuration.withCredentials,
                headers: this.createAuthHeaders(headers),
            }
        ).toPromise();

        return this.processErrors(result);
    }

    public async apiEmployeeGet(): Promise<Array<EmployeeBasicDTO>> {

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];


        let result = this.httpClient.get<Array<EmployeeBasicDTO>>(`${this.basePath}/api/Employee`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: this.createAuthHeaders(headers),
            }
        ).toPromise();

        return this.processErrors(result);
    }

    public async apiEmployeeGetEmployeeDetailGet(employeeId?: string, ): Promise<EmployeeDetailDTO> {


        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (employeeId !== undefined && employeeId !== null) {
            queryParameters = queryParameters.set('employeeId', <any>employeeId);
        }

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];


        let result = this.httpClient.get<EmployeeDetailDTO>(`${this.basePath}/api/Employee/get-employee-detail`,
            {
                params: queryParameters,
                withCredentials: this.configuration.withCredentials,
                headers: this.createAuthHeaders(headers),
            }
        ).toPromise();

        return this.processErrors(result);
    }

}
