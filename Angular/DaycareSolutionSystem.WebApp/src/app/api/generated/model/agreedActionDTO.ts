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
import { ActionDTO } from './actionDTO';


export interface AgreedActionDTO { 
    id?: string;
    clientActionSpecificDescription?: string;
    estimatedDurationMinutes?: number;
    plannedStartTime?: Date;
    plannedEndTime?: Date;
    action?: ActionDTO;
}
