import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from './services/auth-guard.service';
import { SideMenuComponent } from './shared-components/side-menu/side-menu.component';

const routes: Routes = [
  {
    path: '', component: SideMenuComponent, canActivate: [AuthGuardService],
    children: [
      { path: '', loadChildren: './pages/tabs/tabs.module#TabsPageModule' }
    ]
  },
  { path: 'login', loadChildren: './pages/login/login.module#LoginPageModule' },
  { path: 'setup', loadChildren: './pages/setup/setup.module#SetupPageModule' },
];
@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
