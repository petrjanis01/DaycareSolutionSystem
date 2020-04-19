import { query, style, animate, group } from '@angular/animations';

export class SliderAnimation {
    public static left = [
        query(':enter, :leave', style({ position: 'absolute' }), { optional: true }),
        group([
            query(':enter', [style({ transform: 'translateX(-100%)', opacity: 0 }),
            animate('1s ease-out', style({ transform: 'translateX(0%)', opacity: 1 }))], {
                optional: true,
            }),
            query(':leave', [style({ transform: 'translateX(0%)', opacity: 1 }),
            animate('1s ease-out', style({ transform: 'translateX(100%)', opacity: 0 }))], {
                optional: true,
            }),
        ]),
    ];

    public static right = [
        query(':enter, :leave', style({ position: 'absolute' }), { optional: true }),
        group([
            query(':enter', [style({ transform: 'translateX(100%)', opacity: 0 }),
            animate('1s ease-out', style({ transform: 'translateX(0%)', opacity: 1 }))], {
                optional: true,
            }),
            query(':leave', [style({ transform: 'translateX(0%)', opacity: 1 }),
            animate('1s ease-out', style({ transform: 'translateX(-100%)', opacity: 0 }))], {
                optional: true,
            }),
        ]),
    ];
}