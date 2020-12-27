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
    public class UserProfileDialog : ComponentDialog
    {
        private readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;

        public UserProfileDialog(UserState userState)
            : base(nameof(UserProfileDialog))
        {
            _userProfileAccessor = userState.CreateProperty<UserProfile>("UserProfile");

            // This array defines how the Waterfall will execute.
            var waterfallSteps = new WaterfallStep[]
            {
                M1002StepAsync,
                M1003StepAsync,
                M10011StepAsync,
                M10012StepAsync,
                // M10014StepAsync,
                M10015StepAsync,
                M10016StepAsync,
                M10017StepAsync,
                M10018StepAsync,
                M10019StepAsync,
                M10020StepAsync,
                M10021StepAsync,
                M10022StepAsync,
                M10023StepAsync,
                M10024StepAsync,
                M10025StepAsync,
                M10026StepAsync,
                M10027StepAsync,
                M10029StepAsync,
                M10030StepAsync,
                // M10031StepAsync,
                M10035StepAsync,
                // M10036StepAsync,
                // M10037StepAsync,
                // M10038StepAsync
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

        private static async Task<DialogTurnResult> M1002StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions { 
                    Prompt = MessageFactory.Text(" ✍ Do you want a quick exercise you can do right now to help with mood and relaxation? ✍ ") 
                    }, 
                    cancellationToken);
        }

        private static async Task<DialogTurnResult> M1003StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            
            context.ActiveDialog.State["stepIndex"] = (int)context.ActiveDialog.State["stepIndex"] + 2;

            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            
            Random rnd = new Random();
            int skip_index = rnd.Next(0, 7); // generate a number in between 0 and 6

            switch (skip_index){
                case 0: // M10003
                      await stepContext.Context.SendActivityAsync(
                          MessageFactory.Text($"Please find a comfortable seated position. Close your eyes, take a deep breath. Focus on your breath and think about what you are thankful for at this present moment. Take 10 more deep breaths while thinking about things in your life that bring you satisfaction. ")
                          , cancellationToken);
                      break;
                case 1: // M10004
                    await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"Here is a quick (5 min) guided meditation: https://www.youtube.com/watch?v=OCorElLKFQE")
                        , cancellationToken);
                    break;
                case 2: // M10005
                    await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"Here is a quick (1 min) guided meditation: https://www.youtube.com/watch?v=69Bw8rRwUNU")
                        , cancellationToken);
                    break;
                case 3: // M10006 
                    await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"Please take 1 minute to take a few deep and relaxing breaths. ")
                        , cancellationToken);
                    await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"Mindful breathing can help you relax, because it helps your body mimic the physical sensation of being calm.")
                        , cancellationToken);

                    break;
                case 4: // M10008
                    await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"Please find a comfortable seating position. First, exhale completely through your mouth, making a woosh sound. Then, close your mouth gently and inhale through your nose for 4 seconds, hold your breath for 7 seconds and exhale through your mouth for 8 seconds. Repeat this cycle 3 times. ")
                        , cancellationToken);
                    break;
                case 5: // M10009
                    await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"Here are some instructions for a simple breathing exercise: 4-7-8 Breath. https://www.cordem.org/globalassets/files/academic-assembly/2017-aa/handouts/day-three/biofeedback-exercises-for-stress-2---fernances-j.pdf  ")
                        , cancellationToken);
                    break;

                case 6: // M10010
                    await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"Find a comfortable seated position by a window or somewhere scenic. Take a deep breath. Focus on what you see. Bring your attention to one object that you see. Simply notice it without giving any judgement. Slowly start noticing other objects or furniture around you. It is fine if your mind shifts away, just bring your attention back to what you see. ")
                        , cancellationToken);
                    break;
                default:
                    break;
            }

            // time in milliseconds
            // Task.Delay(60000).Wait();
          

            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("✍ Did you like this exercise? Reply with 'yes' or 'no' ✍ ")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> M10011StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("✍ Did you like this exercise? Reply with 'yes' or 'no'  ✍ ")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> M10012StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"Thank you for the feedback!")
                        , cancellationToken);
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("✍  Do you want us to send some questions for you to reflect on? Reply with 'yes' or 'no'  ✍ ")
                }, cancellationToken);
        }

        // private static async Task<DialogTurnResult> M10014StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        // {
        //     Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
        //     // stepContext.ActiveDialog.State["stepIndex"] = 2;
        //     // return await AgeStepAsync(stepContext, cancellationToken);
        //     // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
        //     // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
        //     // return await stepContext.Context.SendActivityAsync(
        //     //             MessageFactory.Text($"Great! You can just think about your answers to these questions, or you can write back with your response. Just remember: This texting program isn't set up to understand what you say. Whatever you write is just for you.")
        //     //             , cancellationToken);
        //     return await stepContext.PromptAsync(nameof(TextPrompt),
        //         new PromptOptions
        //         {
        //             Prompt = MessageFactory.Text("Great! You can just think about your answers to these questions, or you can write back with your response. Just remember: This texting program isn't set up to understand what you say. Whatever you write is just for you.")
        //         }, cancellationToken);
        // }
        private static async Task<DialogTurnResult> M10015StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"Great! You can just think about your answers to these questions, or you can write back with your response. Just remember: This texting program isn't set up to understand what you say. Whatever you write is just for you.")
                        , cancellationToken);
            return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text(" ✍ It can be helpful to give ourselves credit for the things we accomplish, even if they are small. Is there something you've done in the past couple days that helped you move forward? If not, is there a small step you are planning to take in the future?  ✍ ")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> M10016StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            stepContext.Values["M10015"] = (string)stepContext.Result;
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text(" ✍  Thank you for the response! Do you want another question? Reply with 'yes' or 'no'  ✍")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> M10017StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text(" ✍ Do you want another question? Reply with 'yes' or 'no'  ✍ ")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> M10018StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"✍  If you are struggling to find something to be thankful for, you can always start small. It can be a supportive friend, a useful tool, a clean shirt, or a good meal. What is one thing that makes your life better?  ✍ ")
                        , cancellationToken);

            return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("You can write down your response here...")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> M10019StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            stepContext.Values["M10018"] = (string)stepContext.Result;
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("✍ Thank you for the response! Do you want another question? Reply with 'yes' or 'no'  ✍ ")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> M10020StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text(" ✍ Do you want another question? Reply with 'yes' or 'no'  ✍ ")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> M10021StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"✍ When do you feel the most motivated?  ✍ ")
                        , cancellationToken);

            return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("You can write down your response here...")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> M10022StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            stepContext.Values["M10021"] = (string)stepContext.Result;
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text(" ✍ Thank you for the response! Do you want another question? Reply with 'yes' or 'no'  ✍ ")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> M10023StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text(" ✍ Do you want another question? Reply with 'yes' or 'no'  ✍ ")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> M10024StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($" ✍ When do you feel the most relaxed?  ✍ ")
                        , cancellationToken);

            return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text(" You can write down your response here... ")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> M10025StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            stepContext.Values["M10025"] = (string) stepContext.Result;
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("✍ Thank you for the response! Do you want another question? Reply with 'yes' or 'no' ✍ ")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> M10026StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("✍ Do you want another question? Reply with 'yes' or 'no' ✍ ")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> M10027StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"✍  What makes you feel cozy and safe? ✍  ")
                        , cancellationToken);

            return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("You can write down your response here...")
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> M10029StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            stepContext.Values["M10028"] = (string) stepContext.Result;
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"Thank you for the response!")
                        , cancellationToken);
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("✍ Do you want to see a message from another person using the texting program? Reply with 'yes' or 'no' ✍ ")
                }, cancellationToken);
        }


        private static async Task<DialogTurnResult> M10030StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            await stepContext.Context.SendActivityAsync(
                        MessageFactory.Text($"✍  Please pick a topic: ✍ ")
                        , cancellationToken);
            return await stepContext.PromptAsync(nameof(ChoicePrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("Please type # of message (i.e. 4) or the whole message text (i.e. a random message):"),
                    Choices = ChoiceFactory.ToChoices(new List<string> { "a message about being overwhelmed", "a message about handling negative thoughts", 
                    "a message about self-care", "a random message" }),
                }, cancellationToken);
        }

        private static async Task<DialogTurnResult> M10035StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            stepContext.Values["M10030"] = ((FoundChoice)stepContext.Result).Value;
            //Console.WriteLine("Choice Value:" + ((FoundChoice)stepContext.Result).Value);
            switch(stepContext.Values["M10030"]){
                case "a message about being overwhelmed":
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Here's a message from someone else using the texting program: 'It's okay to feel overwhelmed. Remember: One step at a time. Or one day at a time. It really is easier to segment things.'"), cancellationToken);
                    break;
                case "a message about handling negative thoughts":
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Here's a message about dealing with negative thoughts from someone else using the texting program: 'Take a moment to view your circumstances from a different point of view. Pick any point of view you want. See how it feels to look at things differently.'"), cancellationToken);
                    break;
                case "a message about self-care":
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Here's a message about self-care from someone else using the texting program: 'Although they can be painful to witness, other people's problems are not your own. Take some time to remember that the person you are best suited to help is you, just as the person they are best suited to help is them.'"), cancellationToken);
                    break;
                case "a random message":
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Here's a random message from someone else using the texting program: 'Although they can be painful to witness, other people's problems are not your own. Take some time to remember that the person you are best suited to help is you, just as the person they are best suited to help is them.'"), cancellationToken);
                    break;
                default:
                    break;
            }
            // stepContext.ActiveDialog.State["stepIndex"] = 2;
            // return await AgeStepAsync(stepContext, cancellationToken);
            // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
            // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
            return await stepContext.PromptAsync(nameof(ConfirmPrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("✍ Do you want us to send you this message again? Reply with 'yes' or 'no' ✍")
                }, cancellationToken);
        }

        // private static async Task<DialogTurnResult> M10032StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        // {
        //     Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
        //     // stepContext.ActiveDialog.State["stepIndex"] = 2;
        //     // return await AgeStepAsync(stepContext, cancellationToken);
        //     // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
        //     // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
        //     return await stepContext.PromptAsync(nameof(ChoicePrompt),
        //         new PromptOptions
        //         {
        //             Prompt = MessageFactory.Text("Please enter your mode of transport."),
        //             Choices = ChoiceFactory.ToChoices(new List<string> { "Car", "Bus", "Bicycle" }),
        //         }, cancellationToken);
        // }


    //     private static async Task<DialogTurnResult> M10033StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    //     {
    //         Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
    //         // stepContext.ActiveDialog.State["stepIndex"] = 2;
    //         // return await AgeStepAsync(stepContext, cancellationToken);
    //         // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
    //         // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
    //         return await stepContext.PromptAsync(nameof(ChoicePrompt),
    //             new PromptOptions
    //             {
    //                 Prompt = MessageFactory.Text("Please enter your mode of transport."),
    //                 Choices = ChoiceFactory.ToChoices(new List<string> { "Car", "Bus", "Bicycle" }),
    //             }, cancellationToken);
    //     }

    //     private static async Task<DialogTurnResult> M10034StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    //     {
    //         Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
    //         // stepContext.ActiveDialog.State["stepIndex"] = 2;
    //         // return await AgeStepAsync(stepContext, cancellationToken);
    //         // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
    //         // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
    //         return await stepContext.PromptAsync(nameof(ChoicePrompt),
    //             new PromptOptions
    //             {
    //                 Prompt = MessageFactory.Text("Please enter your mode of transport."),
    //                 Choices = ChoiceFactory.ToChoices(new List<string> { "Car", "Bus", "Bicycle" }),
    //             }, cancellationToken);
    //     }

    //     private static async Task<DialogTurnResult> M10035StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    //     {
    //         Console.WriteLine("TransportStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
    //         // stepContext.ActiveDialog.State["stepIndex"] = 2;
    //         // return await AgeStepAsync(stepContext, cancellationToken);
    //         // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
    //         // Running a prompt here means the next WaterfallStep will be run when the user's response is received.
    //         return await stepContext.PromptAsync(nameof(ChoicePrompt),
    //             new PromptOptions
    //             {
    //                 Prompt = MessageFactory.Text("Please enter your mode of transport."),
    //                 Choices = ChoiceFactory.ToChoices(new List<string> { "Car", "Bus", "Bicycle" }),
    //             }, cancellationToken);
    //     }

        private static async Task<DialogTurnResult> M10036StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Console.WriteLine("NameStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
            // stepContext.Values["transport"] = ((FoundChoice)stepContext.Result).Value;

            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Got it! We won't send you this one again."), cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> M10037StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
           
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Got it! We will save this one to send to you later."), cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> M10038StepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Ok, we'll skip this for now!"), cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
           
        }

    //     private static async Task<DialogTurnResult> PictureStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    //     {
    //         Console.WriteLine("PictureStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
    //         stepContext.Values["age"] = (int)stepContext.Result;

    //         var msg = (int)stepContext.Values["age"] == -1 ? "No age given." : $"I have your age as {stepContext.Values["age"]}.";

    //         // We can send messages to the user at any point in the WaterfallStep.
    //         await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);

    //         if (stepContext.Context.Activity.ChannelId == Channels.Msteams)
    //         {
    //             // This attachment prompt example is not designed to work for Teams attachments, so skip it in this case
    //             await stepContext.Context.SendActivityAsync(MessageFactory.Text("Skipping attachment prompt in Teams channel..."), cancellationToken);
    //             return await stepContext.NextAsync(null, cancellationToken);
    //         }
    //         else
    //         {
    //             // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
    //             var promptOptions = new PromptOptions
    //             {
    //                 Prompt = MessageFactory.Text("Please attach a profile picture (or type any message to skip)."),
    //                 RetryPrompt = MessageFactory.Text("The attachment must be a jpeg/png image file."),
    //             };

    //             return await stepContext.PromptAsync(nameof(AttachmentPrompt), promptOptions, cancellationToken);
    //         }
    //     }

    //     private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    //     {
    //         Console.WriteLine("ConfirmStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
    //         stepContext.Values["picture"] = ((IList<Attachment>)stepContext.Result)?.FirstOrDefault();

    //         // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
    //         return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = MessageFactory.Text("Is this ok?") }, cancellationToken);
    //     }

    //     private async Task<DialogTurnResult> SummaryStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    //     {
    //         Console.WriteLine("SummaryStepAsync:" + stepContext.ActiveDialog.State["stepIndex"]);
    //         if ((bool)stepContext.Result)
    //         {
    //             // Get the current profile object from user state.
    //             var userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);

    //             userProfile.Transport = (string)stepContext.Values["transport"];
    //             userProfile.Name = (string)stepContext.Values["name"];
    //             userProfile.Age = (int)stepContext.Values["age"];
    //             userProfile.Picture = (Attachment)stepContext.Values["picture"];

    //             var msg = $"I have your mode of transport as {userProfile.Transport} and your name as {userProfile.Name}";

    //             if (userProfile.Age != -1)
    //             {
    //                 msg += $" and your age as {userProfile.Age}";
    //             }

    //             msg += ".";

    //             await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);

    //             if (userProfile.Picture != null)
    //             {
    //                 try
    //                 {
    //                     await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(userProfile.Picture, "This is your profile picture."), cancellationToken);
    //                 }
    //                 catch
    //                 {
    //                     await stepContext.Context.SendActivityAsync(MessageFactory.Text("A profile picture was saved but could not be displayed here."), cancellationToken);
    //                 }
    //             }
    //         }
    //         else
    //         {
    //             await stepContext.Context.SendActivityAsync(MessageFactory.Text("Thanks. Your profile will not be kept."), cancellationToken);
    //         }

    //         // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is the end.
    //         return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
    //     }

    //     private static Task<bool> AgePromptValidatorAsync(PromptValidatorContext<int> promptContext, CancellationToken cancellationToken)
    //     {
    //         // This condition is our validation rule. You can also change the value at this point.
    //         return Task.FromResult(promptContext.Recognized.Succeeded && promptContext.Recognized.Value > 0 && promptContext.Recognized.Value < 150);
    //     }

    //     private static async Task<bool> PicturePromptValidatorAsync(PromptValidatorContext<IList<Attachment>> promptContext, CancellationToken cancellationToken)
    //     {
    //         if (promptContext.Recognized.Succeeded)
    //         {
    //             var attachments = promptContext.Recognized.Value;
    //             var validImages = new List<Attachment>();

    //             foreach (var attachment in attachments)
    //             {
    //                 if (attachment.ContentType == "image/jpeg" || attachment.ContentType == "image/png")
    //                 {
    //                     validImages.Add(attachment);
    //                 }
    //             }

    //             promptContext.Recognized.Value = validImages;

    //             // If none of the attachments are valid images, the retry prompt should be sent.
    //             return validImages.Any();
    //         }
    //         else
    //         {
    //             await promptContext.Context.SendActivityAsync("No attachments received. Proceeding without a profile picture...");

    //             // We can return true from a validator function even if Recognized.Succeeded is false.
    //             return true;
    //         }
    //     }
    }
}
