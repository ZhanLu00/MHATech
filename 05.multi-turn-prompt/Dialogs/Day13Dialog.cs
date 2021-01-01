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
    public class Day13Dialog : ComponentDialog
    {
        private readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;

        public Day13Dialog(UserState userState)
            : base(nameof(Day13Dialog))
        {
            _userProfileAccessor = userState.CreateProperty<UserProfile>("UserProfile");

            // This array defines how the Waterfall will execute.
            var waterfallSteps = new WaterfallStep[]
            {
                HSP10001StepAsync,
                HSP10009StepAsync,
                HSP10012StepAsync,
 
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

        // assume starts at 9:30 AM
        private static async Task<DialogTurnResult> HSP10001StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Good morning!")
                , cancellationToken);

            Task.Delay(60000).Wait();

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Today we’ll be sending some messages about Help-Seeking.")
                , cancellationToken);

            Random rnd1 = new Random();
            int skip_index_1 = rnd1.Next(0, 2); // generate a number in between 0 and 6

            switch (skip_index_1){
                case 0: // Gain Frame
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"It can feel overwhelming to look for help when you’re feeling down. With so many resources, it can also be difficult to know where to start. One way to get help is to talk to a therapist. Therapists may be able to help by listening, brainstorming coping skills, or directing you to other resources.")
                          , cancellationToken);
                    Task.Delay(480000).Wait();
                
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Even if you aren’t interested in therapy now, or you aren’t sure, learning more about it may help you if you decide you are interested in the future. You can read more here: https://www.mhanational.org/get-professional-help-if-you-need-it")
                          , cancellationToken);       
                    Task.Delay(180000).Wait();

                    break;

                case 1: // Loss Frame
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"It can feel overwhelming to look for help when you’re feeling down. With so many resources, it can also be difficult to know where to start. One way to get help is to talk to a therapist. Therapists may be able to help you identify ways to reduce your low moods.")
                          , cancellationToken);
                    Task.Delay(480000).Wait();
                
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Even if you aren’t interested in therapy now, or you aren’t sure, learning more about it may help you if you decide you are interested in the future. You can read more here: https://www.psychologytoday.com/us/blog/freudian-sip/201102/how-find-the-best-therapist-you")
                          , cancellationToken);
                    Task.Delay(180000).Wait();

                    break;
            }

            Random rnd2 = new Random();
            int skip_index_2 = rnd2.Next(0, 2);

            switch (skip_index_2){
                case 0: // Prompt(Directive)
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Sometimes barriers stand in the way of us reaching out for help. Barriers might be cost, transportation, nervousness about disclosing struggles, or beliefs that therapy wouldn't be very helpful.\n\n Identifying these barriers can be a useful first step. Take some time today just to think about any barriers that are holding you back from seeking therapy.")
                          , cancellationToken);
                    Task.Delay(18000000).Wait();
                    break;

                case 1: // Prompt (NonDirective)
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Sometimes barriers stand in the way of us reaching out for help. Barriers might be cost, transportation, nervousness about disclosing struggles, or beliefs that therapy wouldn't be very helpful. \n\n Identifying these barriers can be a useful first step. You might take some time today just to think about any barriers that are holding you back from seeking therapy.")
                          , cancellationToken);
                    Task.Delay(18000000).Wait();
                    break;
            }

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Don’t forget to try to identify some of the things that might make it difficult for you to seek help.")
                , cancellationToken);

            Task.Delay(60000).Wait();


            Random rnd3 = new Random();
            int skip_index_3 = rnd3.Next(0, 3);

            switch (skip_index_3){
                case 0: // Perspective
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"If you struggle with identifying barriers, don’t worry. Just take it one day at a time.")
                          , cancellationToken);
                    Task.Delay(14400000).Wait();
                    break;

                case 1: // Affirmation
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"You are just as deserving of support as anyone else.")
                          , cancellationToken);
                    Task.Delay(14400000).Wait();
                    break;
                
                
                case 2: // Inspiration
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Today is a new day. Anything is possible!")
                          , cancellationToken);
                    Task.Delay(14400000).Wait();
                    break;
            }

            stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions { 
                    Prompt = MessageFactory.Text("✍ Did you reflect on how you feel about help-seeking today? Reply with “yes” or “no”  ✍") 
                    }, 
                    cancellationToken);
        }

        private static async Task<DialogTurnResult> HSP10009StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result){
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text($"That’s awesome! Just thinking through our own feelings about help-seeking can be an important step. ")
                          , cancellationToken);
                Task.Delay(180000).Wait();
                return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions { 
                    Prompt = MessageFactory.Text("✍ What are some of the barriers that have held you back from seeking therapy? ✍ ") 
                    }, 
                    cancellationToken);
            }
            stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Thank you for the feedback. Maybe you can find an opportunity to reflect more on help-seeking tomorrow."), cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
        private static async Task<DialogTurnResult> HSP10012StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Thank you for sharing."), cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

    
    }
}
