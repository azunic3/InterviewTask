import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DrugApiService } from '../../services/drug-api.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-drug-search',
  templateUrl: './drug-search.component.html',
  styleUrls: ['./drug-search.component.scss']
})
export class DrugSearchComponent implements OnInit {

  query = '';
  loading = false;
  error: string | null = null;
  result: any = null;

  constructor(
    private route: ActivatedRoute,
    private drugApi: DrugApiService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['q']) {
        this.query = params['q'];
        this.search();
      }
    });
  }

  search(): void {
    if (!this.query) return;

    this.loading = true;
    this.error = null;
    this.result = null;

    this.drugApi.searchDrug(this.query).subscribe({
      next: (res) => {
        this.result = res;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to search drug.';
        this.loading = false;
      }
    });
  }
}
