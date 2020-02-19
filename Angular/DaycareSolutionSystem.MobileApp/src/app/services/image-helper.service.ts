import { Injectable } from '@angular/core';
import { Plugins, CameraSource, CameraResultType, CameraPhoto } from '@capacitor/core';
import { ActionSheetController } from '@ionic/angular';
import { ToastService } from './toast.service';

@Injectable({
    providedIn: 'root'
})
export class ImageHelperService {
    constructor(
        private actionSheetController: ActionSheetController,
        private toasterService: ToastService) { }

    public async getImageAsBase64FromDevice(): Promise<string> {
        let image = await this.presentSelectorModal();

        if (image == null) {
            return null;
        }

        let webPath = image.webPath;
        try {
            let imageAsBase64 = await this.fileToBase64(webPath);
            return imageAsBase64;
        } catch (e) {
            this.toasterService.showErrorToast('Image could not be loaded');
            return null;
        }
    }

    // https://blog.shovonhasan.com/using-promises-with-filereader/
    public async fileToBase64(webPath: string): Promise<string> {
        let blob = await (await fetch(webPath)).blob();
        const reader = new FileReader();

        return new Promise((resolve, reject) => {
            reader.onerror = () => {
                reader.abort();
                reject(new DOMException('Problem parsing input file.'));
            };

            reader.onload = () => {
                resolve(reader.result.toString());
            };
            reader.readAsDataURL(blob);
        });
    }

    private async presentSelectorModal(): Promise<CameraPhoto> {
        let image = null;

        let actionSheet = await this.actionSheetController.create({
            buttons: [{
                text: 'Gallery',
                icon: 'image',
                handler: async () => {
                    this.getImageFromDevice(CameraSource.Photos).then(img => {
                        image = img;
                        actionSheet.dismiss();
                    });
                    return false;
                }
            },
            {
                text: 'Camera',
                icon: 'camera',
                handler: () => {
                    this.getImageFromDevice(CameraSource.Camera).then(img => {
                        image = img;
                        actionSheet.dismiss();
                    });
                    return false;
                }
            }]
        });

        await actionSheet.present();
        await actionSheet.onDidDismiss();

        return image;
    }

    private async getImageFromDevice(source: CameraSource): Promise<CameraPhoto> {
        try {
            let image = await Plugins.Camera.getPhoto({
                quality: 90,
                allowEditing: false,
                resultType: CameraResultType.Uri,
                source
            });

            return image;
        } catch (e) {
            return null;
        }
    }
}
