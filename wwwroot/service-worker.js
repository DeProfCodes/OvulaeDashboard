// Cache name (version it for updates)
const CACHE_NAME = 'ovulae-cache-v1.1.102';

// Install event - Caches assets listed in the manifest file
self.addEventListener('install', event =>
{
	event.waitUntil(
		fetch('manifest.json')
			.then(response => response.json())
			.then(manifest =>
			{
				const urlsToCache = [
					'/',
					manifest.start_url || '/',
					...manifest.icons.map(icon => icon.src),
					manifest.theme_color || '',
					manifest.background_color || ''
				].filter(url => url); // Remove any undefined or empty values

				return caches.open(CACHE_NAME).then(cache =>
				{
					return cache.addAll(urlsToCache);
				});
			})
	);
});

// Activate event - Clears old caches
self.addEventListener('activate', event =>
{
	event.waitUntil(
		caches.keys().then(cacheNames =>
		{
			return Promise.all(
				cacheNames
					.filter(cacheName => cacheName !== CACHE_NAME)
					.map(cacheName => caches.delete(cacheName))
			);
		})
	);
});

// Fetch event - Serve cached files or fetch from network
self.addEventListener('fetch', event =>
{
	event.respondWith(
		caches.match(event.request).then(cachedResponse =>
		{
			// Return cached response or fetch from the network
			return cachedResponse || fetch(event.request);
		})
	);
});