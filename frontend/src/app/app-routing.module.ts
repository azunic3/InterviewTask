import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { DrugSearchComponent } from './features/drug-search/drug-search.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'search', component: DrugSearchComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
