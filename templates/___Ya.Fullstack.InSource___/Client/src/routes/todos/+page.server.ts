import { superValidate } from 'sveltekit-superforms/server';
import { z } from 'zod';
import { zod } from 'sveltekit-superforms/adapters';
import { fail } from '@sveltejs/kit';
import { BACKEND_API_HOST } from '$env/static/private';

const UserLoginRequestSchema = z.object({
	email: z.string().min(1, 'Please provide an email!').email('Please provide a valid email!'),
	password: z.string().min(6, 'Your password must have at least 7 characters.'),
});

export const load = async () => {
	const form = await superValidate(zod(UserLoginRequestSchema));
	return { form };
};

export const actions = {
	default: async ({ request, fetch }) => {
		const form = await superValidate(request, zod(UserLoginRequestSchema));

		if (!form.valid) {
			return fail(400, { form });
		}

		const res = await fetch(`${BACKEND_API_HOST}/login`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
			},
			body: JSON.stringify({
				email: form.data.email,
				password: form.data.password,
			}),
		});

		if (!res.ok) {
			return fail(400, { form });
		}

		const { accessToken }: { accessToken: string } = await res.json();
		console.info('SERVER', accessToken);

		return { form, accessToken };
	},
};
