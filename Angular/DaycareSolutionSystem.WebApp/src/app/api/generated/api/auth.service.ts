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

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { LoginDTO } from '../model/loginDTO';

import { Configuration } from '../configuration';


@Injectable({
  providedIn: 'root'
})
export class AuthService extends ApiBase{
    public defaultHeaders = new HttpHeaders();
    public configuration = new Configuration();

    constructor(
        private httpClient: HttpClient,
        router: Router,
        notification: NotifiactionService,
        modal: NgbModal,
        @Optional() configuration: Configuration) {
        super(router, notification, modal);
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

    public async apiAuthLoginPost(LoginDTO?: LoginDTO, ): Promise<any> {


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


        let result = this.httpClient.post<any>(`${this.basePath}/api/Auth/login`,
            LoginDTO,
            {
                withCredentials: this.configuration.withCredentials,
                headers: this.createAuthHeaders(headers),
            }
        ).toPromise();

        return this.processErrors(result);
    }

}
