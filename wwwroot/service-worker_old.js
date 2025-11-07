
const staticCacheName = 'cache-v1.7.0';
const dynamicCacheName = 'runtimeCache-v1.7.0';

// Assets to pre-cache
const precacheAssets = [
	'/',
	'js/pwa.js',
	'manifest.json',
	'fallback.html'
];

// Install Event: Caching static assets
self.addEventListener('install', event =>
{
	event.waitUntil(
		caches.open(staticCacheName).then(cache =>
		{
			return cache.addAll(precacheAssets);
		})
	);
});

// Activate Event: Clearing old caches
self.addEventListener('activate', event =>
{
	event.waitUntil(
		caches.keys().then(keys =>
		{
			return Promise.all(
				keys.filter(key => key !== staticCacheName && key !== dynamicCacheName)
					.map(key => caches.delete(key))
			);
		})
	);
});

self.addEventListener('fetch', event =>
{
	if (event.request.method === 'POST')
	{
		event.respondWith(
			fetch(event.request).catch(() =>
			{
				return new Response('Your request will be sent once you’re back online.', {
					headers: { 'Content-Type': 'text/plain' }
				});
			})
		);
		return;
	}

	// Handle GET requests as usual
	event.respondWith(
		caches.match(event.request).then(cacheRes =>
		{
			return cacheRes || fetch(event.request).then(fetchRes =>
			{
				return caches.open(dynamicCacheName).then(cache =>
				{
					cache.put(event.request, fetchRes.clone());
					return fetchRes;
				});
			});
		}).catch(() =>
		{
			return caches.match('fallback.html');
		})
	);
});