import { error, text } from '@sveltejs/kit';

import { BACKEND_API_HOST } from '$env/static/private';

export const POST = async ({ fetch, request }) => {
	const req: { accessToken: string } = await request.json();

	if (!req || !req.accessToken) {
		return error(400, 'Please provide an access token!');
	}

	const res = await fetch(`${BACKEND_API_HOST}/manage/info`, {
		headers: {
			Authorization: `Bearer ${req.accessToken}`,
		},
	});

	if (res.ok) {
		return text(((await res.json()) as { email: string }).email);
	}

	return error(401, 'Invalid access token!');
};
