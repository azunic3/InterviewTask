import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DrugSearchComponent } from './features/drug-search/drug-search.component';

const routes: Routes = [
  { path: '', component: DrugSearchComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
