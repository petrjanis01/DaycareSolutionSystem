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
import { BaseUrlService } from 'src/app/services/base-url.service';
import { NavController } from '@ionic/angular';
import { ToastService } from 'src/app/services/toast.service';

import { CustomHttpUrlEncodingCodec } from '../encoder';


import { Configuration } from '../configuration';


@Injectable({
  providedIn: 'root'
})
export class TestService extends ApiBase{
    public defaultHeaders = new HttpHeaders();
    public configuration = new Configuration();

    constructor(
        private httpClient: HttpClient,
        baseUrlService: BaseUrlService,
         nav: NavController,
        toast: ToastService,
        @Optional() configuration: Configuration) {
        super(baseUrlService, nav, toast);
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

    public async apiTestTestPost(): Promise<any> {

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


        let result = this.httpClient.post<any>(`${this.basePath}/api/Test/test`,
            null,
            {
                withCredentials: this.configuration.withCredentials,
                headers: this.checkUserAndCreateAuthHeaders(headers),
            }
        ).toPromise();

        return this.processResponse(result);
    }

}
