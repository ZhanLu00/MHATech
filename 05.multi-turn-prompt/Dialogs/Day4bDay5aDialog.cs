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
    public class Day4bDialog : ComponentDialog
    {
        private readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;

        public Day3bDialog(UserState userState)
            : base(nameof(Day4bDialog))
        {
            _userProfileAccessor = userState.CreateProperty<UserProfile>("UserProfile");

            // This array defines how the Waterfall will execute.
            var waterfallSteps = new WaterfallStep[]
            {
                BAP20001StepAsync,
                BAP20009StepAsync,
                BAP10013StepAsync
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
        private static async Task<DialogTurnResult> BAP20001StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Good morning!")
                , cancellationToken);

            Task.Delay(60000).Wait();

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Today we’ll be sending some messages about Behavioral Activation.")
                , cancellationToken);

            Random rnd1 = new Random();
            int skip_index_1 = rnd1.Next(0, 2); // generate a number in between 0 and 2

            switch (skip_index_1){
                case 0: // Gain Frame
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Over time, Behavioral Activation can help people feel more satisfied and less down, just by incorporating rewarding activities into their routines.")
                          , cancellationToken);
                    break;

                case 1: // Loss Frame
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Over time, Behavioral Activation can help people get out of a funk or reduce symptoms of depression, just by incorporating rewarding activities into their routines.")
                          , cancellationToken);
                    break;
            }
            
            Task.Delay(480000).Wait();

            Random rnd2 = new Random();
            int skip_index_2 = rnd2.Next(0, 2);

            switch (skip_index_2){
                case 0: // Prompt(Directive)
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Today, let’s focus on noticing how your mood changes when you engage in small, rewarding activities.\nMake a plan to do one pleasant activity later today. Take notice of how you feel immediately before and after you do that activity.")
                          , cancellationToken);
                    break;

                case 1: // Prompt (NonDirective)
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Today, you could focus on noticing how your mood changes when you engage in small, rewarding activities.\nConsider making a plan to do one pleasant activity later today. You might try to take notice of how you feel immediately before and after you do that activity.")
                          , cancellationToken);
                    break;
            }

            Task.Delay(180000).Wait();
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"✍ Write back the activity you are planning  ✍")
                , cancellationToken);
            
            Task.Delay(17820000).Wait();
            // 2:39pm
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Don’t forget to try a pleasant activity today. Notice your mood before and after.")
                , cancellationToken);

            Task.Delay(60000).Wait();


            Random rnd3 = new Random();
            int skip_index_3 = rnd3.Next(0, 3);

            switch (skip_index_3){

                case 0: // Affirmation
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"You can do this just as well as anyone else.")
                          , cancellationToken);
                    break;
                
                
                case 1: // Inspiration
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Today is a new day to start fresh. Anything is possible!")
                          , cancellationToken);
                    break;

                case 2: // Perspective
                    await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"If you struggle finding time for this one, don’t worry. Just getting through the day is enough too.")
                          , cancellationToken);
                    break;
            }

            // 18:40
            Task.Delay(21600000).Wait();
            stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions { 
                    Prompt = MessageFactory.Text("✍ Were you able to do a pleasant activity today? Reply with “yes” or “no”  ✍") 
                    }, 
                    cancellationToken);
        }

        private static async Task<DialogTurnResult> BAP20009StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {   
            if ((bool)stepContext.Result){
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"✍ Wonderful! You deserve some recognition for taking that step. Did you notice any improvement in your mood? Reply with “yes” or “no” ✍"), cancellationToken);
                if ((bool)stepContext.Result){
                    stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Great! Seems like you’ve done an activity that can be pretty rewarding."), cancellationToken);
                    Random rnd4 = new Random();
                    int skip_index_4 = rnd4.Next(0, 2);

                    switch (skip_index_4){
                        case 0: 
                            return await stepContext.Context.SendActivityAsync(
                                MessageFactory.Text($"✍ How can you make this activity a bigger part of your daily routine? ✍")
                                , cancellationToken);
                        case 1: 
                            return await stepContext.Context.SendActivityAsync(
                                MessageFactory.Text($"✍ How did it feel to keep track of your mood today? ✍")
                                , cancellationToken);
                    }
                } else{
                    return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                        new PromptOptions { 
                            Prompt = MessageFactory.Text("Not every activity will lift your mood every time. But it can still be a helpful practice to notice how different activities affect our moods. You might try out a different activity next time, and see if you get a different result.") 
                            }, 
                            cancellationToken); 
                    
                }
            } else {
                stepContext.ActiveDialog.State["stepIndex"] = (int)stepContext.ActiveDialog.State["stepIndex"];
                return await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Maybe you can find an opportunity to do something enjoyable tomorrow! Take a few minutes now to think about what you’ll do and when you’ll do it."), cancellationToken);
            }
        }
    }
}
