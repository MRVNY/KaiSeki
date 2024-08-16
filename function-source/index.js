const functions = require('@google-cloud/functions-framework');
const { GoogleGenerativeAI, HarmBlockThreshold, HarmCategory } = require("@google/generative-ai");

const en_1 = "If the input is hex code image, first extract the main Japanese text, then translate the given Japanese sentence, and parse and analyze the grammar of every word. Just plain text. No descriptions and explanations. Return only raw JSON object without markdown. No markdown format.\nAll the grammar, except for the quoted, should be in English and should include some insights\nThe parsing for words and phrases should not fragment too much\n [Structure of the JSON object] \n ";
const en_2 ="{\"<original Japanese sentence>\": {\"translation\": \"<translation of the sentence>\",\"grammar\": \"<complete grammar analysis of the sentence, taking account the structure and context>\",\"phrases\": {\"<phrase1>\": {\"translation\": \"<translation of the phrase>\",\"grammar\": \"<grammar of the phrase>\",\"words\":{\"<word1>\":{\"definition\": \"<definition of the word>\", \"furigana\": \"<pronunciation of the word>\", \"original_form\": \"<original form of the word>\", \"type\": \"<type of the word (verb, noun, etc.)>\", \"grammar\": \"<what form it is, function of the word in the sentence, explanation of the informal form, etc.>\"},\"<word2>\": \"etc.\"}},\"<phrase2>\": \"etc.\"}}}";
// const en_2 ="{\"<original Japanese sentence>\": {\"Translation\": \"<translation of the sentence>\",\"Grammar\": \"<complete grammar analysis of the sentence, taking account the structure and context>\",\"phrases\": {\"<phrase1>\": {\"Translation\": \"<translation of the phrase>\",\"Grammar\": \"<grammar of the phrase>\",\"words\":{\"<word1>\":{\"Definition\": \"<definition of the word>\", \"Pronunciation\": \"<pronunciation of the word>\", \"Original Form\": \"<original form of the word>\", \"Type\": \"<type of the word (verb, noun, etc.)>\", \"Grammar\": \"<what form it is, function of the word in the sentence, explanation of the informal form, etc.>\"},\"<word2>\": \"etc.\"}},\"<phrase2>\": \"etc.\"}}}";


const generationConfig = {
  temperature: 1,
  topP: 0.95,
  topK: 64,
  maxOutputTokens: 8192,
  responseMimeType: "application/json"
};

const safetySetting = [
  {
    category: HarmCategory.HARM_CATEGORY_SEXUALLY_EXPLICIT,
    threshold: HarmBlockThreshold.BLOCK_NONE,
  }
];
    
const genAI = new GoogleGenerativeAI("AIzaSyA1t3lHBJgy_cHr5ChId2xHBOqbHpaYo-E");

const generativeModel2 = genAI.getGenerativeModel({
    model: "gemini-1.5-flash",
    generationConfig,
    safetySetting,
    systemInstruction: en_1 + en_2
});


functions.http('kaiseki', async (req, res) => {
    // const result = await generativeModel2.generateContent(req.body.name || req.body.name);
    // console.log(req.body.name)
    // const base64string = Buffer.from(req.body.name, 'hex').toString('base64');
    // console.log(base64string)
    const imageParts = [{
        inlineData: {
            data: req.body.name,
            mimeType: "image/jpeg"
        },
    }];
    const result = await generativeModel2.generateContent(['image:',imageParts]);
    const response = await result.response;
    const text = response.text();
    res.send(text);
});

functions.http('text', async (req, res) => {
    const result = await generativeModel2.generateContent(req.body.name || req.body.name);
    const response = await result.response;
    const text = response.text();
    res.send(text);
});
