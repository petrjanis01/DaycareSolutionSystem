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
import { AgreedActionBasicDTO } from './agreedActionBasicDTO';
import { DayOfWeek } from './dayOfWeek';


export interface AgreedActionsForDayDTO { 
    day?: DayOfWeek;
    agreedActions?: Array<AgreedActionBasicDTO>;
}
