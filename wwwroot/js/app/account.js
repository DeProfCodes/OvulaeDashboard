$(document).ready(function () 
{
	/*** QUOTES TYPING EFFECT ***/
	const quotes = [
		{ text: "The biggest risk is not taking any risk. In a world that’s changing quickly, the only strategy that is guaranteed to fail is not taking risks.", author: "Mark Zuckerberg" },
		{ text: "The best way to predict the future is to create it.", author: "Peter Drucker" },
		{ text: "If you don’t find a way to make money while you sleep, you will work until you die.", author: "Warren Buffett" },
		{ text: "Investing is laying out money now to get more money back in the future.", author: "Warren Buffett" },
		{ text: "Opportunities come infrequently. When it rains gold, put out the bucket, not the thimble.", author: "Warren Buffett" },
		{ text: "The greatest investment you can make is in yourself and your ability to take action.", author: "Tony Robbins" },
		{ text: "Money is a terrible master but an excellent servant—learn to make it work for you.", author: "P.T. Barnum" },
		{ text: "Your financial freedom begins the day you realize you must take control of your investments.", author: "Robert Kiyosaki" }
	];

	let quoteIndex = 0;

	function typeQuote() 
	{
		let $quoteContainer = $("#quote-container");

		// Keep only 2 quotes at a time
		if ($quoteContainer.children().length >= 2) 
		{
			$quoteContainer.children().first().remove();
		}

		let quoteText = `"${quotes[quoteIndex].text}"`;
		let authorText = `- ${quotes[quoteIndex].author}`;
		let textIndex = 0;
		let authorIndex = 0;

		let $quoteWrapper = $("<div class='quote-wrapper'></div>");
		let $quote = $("<p class='quote'></p>");
		let $cursor = $("<span class='cursor'>|</span>");
		let $author = $("<div class='quote-author'></div>");

		$quoteWrapper.append($quote).append($cursor).append($author);
		$quoteContainer.append($quoteWrapper);

		function typeWriter() 
		{
			if (textIndex < quoteText.length) 
			{
				$quote.append(quoteText.charAt(textIndex));
				textIndex++;
				setTimeout(typeWriter, 50);
			}
			else 
			{
				$cursor.hide();
				typeAuthor();
			}
		}

		function typeAuthor() 
		{
			if (authorIndex < authorText.length) 
			{
				$author.append(authorText.charAt(authorIndex));
				authorIndex++;
				setTimeout(typeAuthor, 50);
			}
			else 
			{
				quoteIndex = (quoteIndex + 1) % quotes.length;
				setTimeout(typeQuote, 3000);
			}
		}
		setTimeout(typeWriter, 1000);
	}

	typeQuote();


	/*** TYPING EFFECT FOR HEADER MESSAGE ***/
	const phrases = [
		{ text: "Welcome", color: "gold" },
		{ text: "To Your Future", color: "deepskyblue" },
		{ text: "Where You are", color: "gold" },
		{ text: "FINANCIALLY FREE", color: "lime" }
	];

	const finalMessage = "Your Future SELF THANKS YOU";
	const finalMessageColor = "deepskyblue";

	let phraseIndex = 0;
	let charIndex = 0;
	let isErasing = false;
	const typingSpeed = 100;
	const eraseSpeed = 50;
	const holdTime = 300;
	const restartDelay = 5000;

	function typeEffect()
	{
		let $typingText = $("#typing-text");

		if (!isErasing)
		{
			// Typing effect
			$typingText.text(phrases[phraseIndex].text.substring(0, charIndex));
			charIndex++;

			if (charIndex > phrases[phraseIndex].text.length)
			{
				setTimeout(() =>
				{
					isErasing = true;
					typeEffect();
				}, holdTime);
			} else
			{
				setTimeout(typeEffect, typingSpeed);
			}
		} else
		{
			// Erasing effect
			$typingText.text(phrases[phraseIndex].text.substring(0, charIndex));
			charIndex--;

			if (charIndex < 0)
			{
				isErasing = false;
				phraseIndex++;

				if (phraseIndex >= phrases.length)
				{
					// Show final message, then restart
					$typingText.css("color", finalMessageColor).text(finalMessage).css("border-right", "none");

					setTimeout(() =>
					{
						phraseIndex = 0;
						charIndex = 0;
						$typingText.css("border-right", "3px solid white");
						$typingText.css("color", phrases[phraseIndex].color);
						typeEffect();
					}, restartDelay);

					return;
				}

				// Reset charIndex and apply new color
				charIndex = 0;
				$typingText.css("color", phrases[phraseIndex].color);
			}
			setTimeout(typeEffect, eraseSpeed);
		}
	}

	// Start typing after a slight delay
	setTimeout(typeEffect, 1000);
});



function typeQuote()
{
	let $quoteContainer = $("#quote-container");

	// Keep only 2 quotes at a time
	if ($quoteContainer.children().length >= 2)
	{
		$quoteContainer.children().first().remove();
	}

	let quoteText = `"${quotes[quoteIndex].text}"`;
	let authorText = `- ${quotes[quoteIndex].author}`;
	let textIndex = 0;
	let authorIndex = 0;

	let $quoteWrapper = $("<div class='quote-wrapper'></div>");
	let $quote = $("<p class='quote'></p>").css("color", "#ffcc00"); // Gold for quote
	let $cursor = $("<span class='cursor'>|</span>");
	let $author = $("<div class='quote-author'></div>").css("color", "deepskyblue"); // Blue for author

	$quoteWrapper.append($quote).append($cursor).append($author);
	$quoteContainer.append($quoteWrapper);

	function typeWriter()
	{
		if (textIndex < quoteText.length)
		{
			$quote.append(quoteText.charAt(textIndex));
			textIndex++;
			setTimeout(typeWriter, 50);
		} else
		{
			$cursor.hide();
			typeAuthor();
		}
	}

	function typeAuthor()
	{
		if (authorIndex < authorText.length)
		{
			$author.append(authorText.charAt(authorIndex));
			authorIndex++;
			setTimeout(typeAuthor, 50);
		} else
		{
			quoteIndex = (quoteIndex + 1) % quotes.length;
			setTimeout(typeQuote, 3000);
		}
	}

	setTimeout(typeWriter, 1000);
}