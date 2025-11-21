import './polyfills';
import './styles.scss';
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { appConfig } from './app/app.config';

bootstrapApplication(AppComponent, appConfig)
	.then(() => {
		console.log('Angular bootstrapped');
	})
	.catch(err => console.error(err));
