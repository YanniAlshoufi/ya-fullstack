<script lang="ts">
	type Todo = {
		text: string;
		isChecked: boolean;
	};

	import { superForm } from 'sveltekit-superforms';
	import type { PageData } from './$types';
	import { browser } from '$app/environment';

	const { data }: { data: PageData } = $props();

	let currentUserName: string | null = $state(null);
	let isEmailOrPasswordIncorrect: boolean = $state(false);

	const { form, errors, constraints, enhance } = superForm(data.form, {
		onUpdate: async ({ result }) => {
			if (result.type === 'failure') {
				isEmailOrPasswordIncorrect = true;
				return;
			}

			isEmailOrPasswordIncorrect = false;

			if (browser) {
				window.sessionStorage.setItem('accessToken', result.data.accessToken);
			}
			await setUserFromServer();
		},
	});

	let todos: Todo[] = $state([]);

	$effect(() => {
		setUserFromServer();

		if (!browser) {
			return;
		}

		todos = JSON.parse(window.localStorage.getItem('todos') ?? '[]');
	});

	async function setUserFromServer() {
		const accessToken: string | null = browser
			? (window.sessionStorage.getItem('accessToken') ?? null)
			: null;

		if (accessToken === null) {
			return;
		}

		const res = await fetch('/todos/api/user-email', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
			},
			body: JSON.stringify({ accessToken }),
		});

		if (!res.ok) {
			return;
		}

		currentUserName = await res.text();
	}

	let todoToAddText = $state('');
	function onAddTodo() {
		const trimmedText = todoToAddText.trim();

		if (trimmedText === '') {
			todoToAddText = trimmedText;
			return;
		}

		todos.push({
			text: trimmedText,
			isChecked: false,
		});
	}

	function onRemoveTodo(todo: Todo) {
		const idx = todos.indexOf(todo);

		if (idx === -1) {
			return;
		}

		todos.splice(idx, 1);
	}

	$effect(() => {
		if (browser) {
			window.localStorage.setItem('todos', JSON.stringify(todos));
		}
	});
</script>

<header class="flex h-40 items-center justify-center bg-blue-700">
	<div class="flex h-4/5 w-4/5 items-center justify-center gap-60">
		<form method="post" use:enhance class="flex gap-10 [&>label]:gap-2">
			<label class="flex flex-col">
				Email
				<input
					required
					type="email"
					name="email"
					aria-invalid={$errors.email ? 'true' : undefined}
					bind:value={$form.email}
					{...$constraints.email}
				/>
				{#if $errors.email}
					<small class="text-sm text-red-400">{$errors.email}</small>
				{/if}
			</label>
			<label class="flex flex-col">
				Password
				<input
					required
					type="password"
					name="password"
					aria-invalid={$errors.password ? 'true' : undefined}
					bind:value={$form.password}
					{...$constraints.password}
				/>
				{#if $errors.password}
					<small class="text-sm text-red-400">{$errors.password}</small>
				{/if}
			</label>

			{#if isEmailOrPasswordIncorrect}
				<small class="text-sm text-red-400">
					Looks like the email or password (or both) you entered are incorrect.
				</small>
			{/if}

			<input type="submit" class="hidden" />
		</form>
		<div class="flex items-center justify-center gap-2">
			<p class="text-xl">User:</p>
			<h3 class="text-2xl font-bold">{currentUserName ?? 'No One'}</h3>
		</div>
	</div>
</header>

<main class="flex flex-col items-center justify-center">
	<div class="flex w-1/2 items-center justify-center bg-gray-900 py-10">
		<div class="flex flex-col gap-5">
			<h2 class="text-center text-3xl">Todos</h2>
			<ul class="flex flex-col gap-2">
				{#each todos as todo}
					<li>
						<label>
							<input type="checkbox" bind:checked={todo.isChecked} />
							{todo.text}
						</label>

						<button
							onclick={() => onRemoveTodo(todo)}
							class="icon-[mingcute--delete-fill] text-red-500"
						></button>
					</li>
				{/each}
			</ul>
			<form class="flex" onsubmit={onAddTodo}>
				<input type="text" bind:value={todoToAddText} />
				<button
					class="flex aspect-square items-center justify-center border border-red-500
				bg-red-400 px-2"
				>
					<span class="icon-[mingcute--plus-fill]"></span>
				</button>
			</form>
		</div>
	</div>
</main>
