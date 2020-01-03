import { Component, Input } from '@angular/core';
import { Client } from 'src/app/services/clients/client';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'client-card',
  templateUrl: './client-card.component.html',
  styleUrls: ['./client-card.component.scss'],
})
export class ClientCardComponent {
  @Input() client: Client;

  constructor(private router: Router, private route: ActivatedRoute) { }

  public navigateToDetail() {
    this.router.navigate(['client-detail', this.client.id], { relativeTo: this.route });
  }
}
