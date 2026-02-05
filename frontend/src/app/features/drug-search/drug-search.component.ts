import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DrugApiService } from '../../services/drug-api.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-drug-search',
  templateUrl: './drug-search.component.html',
  styleUrls: ['./drug-search.component.scss'],
})
export class DrugSearchComponent implements OnInit {
  query = '';
  loading = false;
  error: string | null = null;
  result: any = null;
  allergensInput = ''; // npr "penicillin, sulfa"
  notifyEmail = '';
  notifyState: 'idle' | 'loading' | 'success' | 'exists' | 'error' = 'idle';
  notifyMessage = '';

  constructor(
    private route: ActivatedRoute,
    private drugApi: DrugApiService,
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
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
      },
    });
  }

get allergensList(): string[] {
  return this.allergensInput
    .split(',')
    .map(x => x.trim())
    .filter(Boolean);
}

notifyMe(): void {
  if (!this.result?.drugKey) return;
  if (!this.notifyEmail) return;

  this.notifyState = 'loading';
  this.notifyMessage = '';

  this.drugApi.notifyAvailability(this.result.drugKey, this.notifyEmail).subscribe({
    next: (res) => {
      this.notifyState = 'success';
      this.notifyMessage = res?.message
        ?? 'Thanks! We saved your request and will notify you when it’s available.';
    },
    error: (err) => {
      this.notifyState = 'error';
      this.notifyMessage =
        err?.error
          ? (typeof err.error === 'string' ? err.error : err.error?.message) 
          : 'Could not save your request. Please try again.';
    },
  });
}

get allergyStatus() {
  const allergens = this.allergensList;
  if (!this.result || allergens.length === 0) return { type: 'none', matched: [] as string[] };

  const hay =
    `${this.result.safetyText ?? ''} ${this.result.drug?.activeIngredient ?? ''}`.toLowerCase();

  const matched = allergens.filter(a => hay.includes(a.toLowerCase()));
  return { type: matched.length ? 'match' : 'safe', matched };
}


}
