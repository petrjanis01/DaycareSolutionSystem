import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from './services/auth-guard.service';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./pages/tabs/tabs.module').then(m => m.TabsPageModule),
    canActivate: [AuthGuardService]
  },
  { path: 'login', loadChildren: './pages/login/login.module#LoginPageModule' },
  { path: 'setup', loadChildren: './pages/setup/setup.module#SetupPageModule' },
  { path: 'client-action-detail', loadChildren: './pages/schedule/client-action-detail/client-action-detail.module#ClientActionDetailPageModule' }
];
@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}
