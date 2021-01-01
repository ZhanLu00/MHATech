// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;

namespace Microsoft.BotBuilderSamples
{
    public class Day9Dialog : ComponentDialog
    {
        private readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;

        public Day9Dialog(UserState userState)
            : base(nameof(Day9Dialog))
        {
            _userProfileAccessor = userState.CreateProperty<UserProfile>("UserProfile");

            // This array defines how the Waterfall will execute.
            var waterfallSteps = new WaterfallStep[]
            {
                S10002StepAsync,
                S10003StepAsync,
                S10005StepAsync,
                S10006StepAsync,
                S10014StepAsync,
                S10018StepAsync,
                S10019StepAsync,
                S10028StepAsync,
                S10031StepAsync,
            };

            // Add named dialogs to the DialogSet. These names are saved in the dialog state.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            // AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>), AgePromptValidatorAsync));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            // AddDialog(new AttachmentPrompt(nameof(AttachmentPrompt), PicturePromptValidatorAsync));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private static async Task<DialogTurnResult> S10002StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Good morning!")
                , cancellationToken);

            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions { 
                    Prompt = MessageFactory.Text("✍ Want to hear a story from another person using this texting program? ✍") 
                    }, 
                    cancellationToken);

        }

        private static async Task<DialogTurnResult> S10003StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result){
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
                return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions { 
                    Prompt = MessageFactory.Text("✍ Great! Let’s find a good topic. Do you ever feel like you can’t find motivation to do what you need to do? Reply with “Yes” or “No” ✍") 
                    }, 
                    cancellationToken);
            }
            stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Got it, we’ll skip this today."), cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

        private static async Task<DialogTurnResult> S10005StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result == null || ((bool)stepContext.Result)) {
                // Skip from 3 to 19
                // stepContext.Values["10003D"] = true;
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"] + 3;
                return await S10019StepAsync(stepContext, cancellationToken);
            }
            stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions { 
                    Prompt = MessageFactory.Text("✍ Do you ever feel like everything is going wrong at once? Reply with “Yes” or “No”  ✍") 
                    }, 
                    cancellationToken);
        }

        private static async Task<DialogTurnResult> S10006StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result == null || !(bool)stepContext.Result){
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("Everyone has a unique set of challenges and experiences, but there are many others out there who can relate to what you're going through. Click here anytime to read others' stories about how they are coping with hard feelings and situations: https://www.mhanational.org/mentalillnessfeelslike")
                    , cancellationToken);
                Task.Delay(30000).Wait();
            }

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text("Here's one person's experience of feeling this way: \n\n I got a low grade on an assignment for my econ class, and I was feeling really down about it. It set me off thinking I can't do anything right. I had worked really hard on the assignment, and I was already having a bad week, fighting with my Dad, being broke, and just feeling frustrated about a bunch of things. ")
                , cancellationToken);
                        
            Task.Delay(30000).Wait();

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text("I was beating myself up about all of this for hours. Sitting on my bed and thinking about how big a failure I am, and finding ways that all of the things going wrong were my fault.")
                , cancellationToken);
                        
            Task.Delay(30000).Wait();
            
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text("I did end up looking for a way to snap myself out of it. I actually texted a friend who I haven't talked to in a while. We have the kind of relationship where we can talk about almost anything. I don't have that with a lot of people. We're living far away from each other so unfortunately we don't talk very much anymore. ")
                , cancellationToken);
                        
            Task.Delay(30000).Wait();

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text("I mentioned the assignment and all of the other things, and she was really sweet about it. She said she understood why I was frustrated. She also said something like, \" Everyone has those weeks where everything is going wrong all at once. It's part of being human.\"")
                , cancellationToken);
            
            Task.Delay(30000).Wait();

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text("I felt a lot better after I talked to her. It was nice just to be listened to by someone. Sometimes I don't reach out because I don't want to burden my friends, but I'm usually glad when I do.")
                , cancellationToken);
                        
            Task.Delay(30000).Wait();

            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions { 
                    Prompt = MessageFactory.Text("✍ Can you relate to this person’s experience? Reply with “Yes” or “No” ✍") 
                    }, 
                    cancellationToken);
        }

        private static async Task<DialogTurnResult> S10014StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result == null || (!(bool)stepContext.Result)) {
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Thank you for the feedback!"), cancellationToken);
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            }
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Thank you for the feedback!")
                , cancellationToken);
            
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"It is not uncommon to see a single, unpleasant event as part of a never-ending pattern of defeat, but when we do this, we are often overlooking the many things that actually go well.  ")
                , cancellationToken);

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"This is a thinking trap of \"overgeneralization.\" You can read about it here: http://cogbtherapy.com/cbt-blog/cognitive-distortions-overgeneralizing")
                , cancellationToken);

            return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions { 
                    Prompt = MessageFactory.Text("✍ Can you think of a time when you overgeneralized from one or two things that went wrong? ✍") 
                    }, 
                    cancellationToken);
        }

        private static async Task<DialogTurnResult> S10018StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Thank you for sharing."), cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

        private static async Task<DialogTurnResult> S10019StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text("Here's one person's experience of feeling this way: \n\n I woke up this morning and just didn't want to face the day at all. I had to write several emails for work, and finish putting together a presentation. I felt like I really didn't care and didn't have it in me.")
                , cancellationToken);
            
            Task.Delay(30000).Wait();

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text("I started scrolling Instagram, I needed to ease into the morning. I didn’t mean to, but spent over an hour on Instagram just scrolling. I felt worse. I was even less motivated to do my work and really frustrated with myself for wasting time. ")
                , cancellationToken);
                        
            Task.Delay(30000).Wait();

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text("I was basically giving up on getting anything done, I’ve done this before, just said to myself, ‘well, it’s just going to be one of those days.’ And made peace with the fact that I wouldn’t get anything done.")
                , cancellationToken);
                        
            Task.Delay(30000).Wait();
            
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text("But then I caught myself. The day wasn’t even half over. It was only 11am. While it wasn’t the start to the morning I’d wanted, there was plenty of time to get some things done – even if I couldn’t get everything done.")
                , cancellationToken);
                        
            Task.Delay(30000).Wait();

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text("I noticed my \"all-or-nothing\" thinking, a trap that I get stuck in a lot. All-or-nothing thinking is when there is no room for middle ground. It’s either ALL good or ALL bad. If one small thing doesn’t go the way I want, it’s as if the whole day is spoiled. So, I end up being extra hard on myself even when it doesn’t fit the facts of the situation.")
                , cancellationToken);
            
            Task.Delay(30000).Wait();

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text("I was able to catch my all-or-nothing thinking and challenge it. I actually had a pretty good day after that. It’s true I didn’t get ALL the work done I’d planned, but I definitely got some done and was able to feel like I’d moved towards my goals.")
                , cancellationToken);
                        
            Task.Delay(30000).Wait();

            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions { 
                    Prompt = MessageFactory.Text("✍ Can you relate to this person’s experience? Reply with “Yes” or “No” ✍") 
                    }, 
                    cancellationToken);
        }

        private static async Task<DialogTurnResult> S10028StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result == null || (!(bool)stepContext.Result)) {
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Thank you for the feedback!"), cancellationToken);
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            }

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Thank you for the feedback!")
                , cancellationToken);
            
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"It is very common to fall into thinking traps like the all-or-nothing trap.")
                , cancellationToken);

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"See if you can notice these kinds of traps and challenge them.")
                , cancellationToken);

            return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions { 
                    Prompt = MessageFactory.Text("✍ Is there a situation that you’ve encountered recently where you fell into the all-or-nothing trap? What important facts were being left out of your thinking? ✍") 
                    }, 
                    cancellationToken);
        }
        private static async Task<DialogTurnResult> S10031StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Thank you for sharing."), cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

    
    }
}
