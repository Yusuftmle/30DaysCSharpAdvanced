using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

public class BlogGenerator
{
    private readonly Kernel _kernel;

    public BlogGenerator(Kernel kernel)
    {
        _kernel = kernel;
    }

    // -----------------------------
    // BlogPost Title -> Outline List
    // -----------------------------
    public async Task<List<string>> GenerateOutlineAsync(string blogTitle)
    {
        string prompt = $@"Write 5 clear blog outline titles for this topic: {blogTitle}
Format as numbered list (1. 2. 3. etc.)";

        var result = await _kernel.InvokePromptAsync(prompt);
        var content = result.GetValue<string>() ?? string.Empty;

        return content
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Trim())
            .ToList();
    }

    // -----------------------------
    // Outline Title -> ContentBlock
    // -----------------------------
    public async Task<string> GenerateContentBlockAsync(string outlineTitle)
    {
        string prompt = $@"Write a detailed, informative paragraph (3-5 sentences) for this blog subheading: 
{outlineTitle}

Make it engaging and provide specific details or examples.";

        var result = await _kernel.InvokePromptAsync(prompt);
        return result.GetValue<string>() ?? string.Empty;
    }

    // -----------------------------
    // ContentBlock -> Review
    // -----------------------------
    public async Task<(string Notes, string Status)> ReviewContentAsync(string content)
    {
        string prompt = $@"Review this paragraph and provide constructive feedback:

{content}

Provide:
1. What works well
2. Specific suggestions for improvement
3. Overall assessment (Good/Needs Work/Excellent)

Format your response clearly with these sections.";

        var result = await _kernel.InvokePromptAsync(prompt);
        var reviewText = result.GetValue<string>() ?? string.Empty;

        // Extract status from the review (simple parsing)
        var status = ExtractStatusFromReview(reviewText);

        return (reviewText, status);
    }

    // -----------------------------
    // Sequential Chain: Title -> Full Blog
    // -----------------------------
    public async Task<string> GenerateFullBlogAsync(string blogTitle)
    {
        // Step 1: Generate outline
        var outline = await GenerateOutlineAsync(blogTitle);

        // Step 2: Generate content for each outline item
        var contentBlocks = new List<string>();
        foreach (var outlineItem in outline)
        {
            var content = await GenerateContentBlockAsync(outlineItem);
            contentBlocks.Add($"## {outlineItem}\n\n{content}");
        }

        // Step 3: Combine into full blog
        var fullBlog = $"# {blogTitle}\n\n{string.Join("\n\n", contentBlocks)}";

        return fullBlog;
    }

    // -----------------------------
    // Parallel Chain: Generate multiple content blocks simultaneously
    // -----------------------------
    public async Task<List<string>> GenerateContentBlocksParallelAsync(List<string> outlineTitles)
    {
        var tasks = outlineTitles.Select(title => GenerateContentBlockAsync(title));
        var results = await Task.WhenAll(tasks);
        return results.ToList();
    }

    // -----------------------------
    // Conditional Chain: Review and regenerate if needed
    // -----------------------------
    public async Task<string> GenerateContentWithReviewAsync(string outlineTitle, int maxAttempts = 2)
    {
        string content = string.Empty;

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            content = await GenerateContentBlockAsync(outlineTitle);
            var (notes, status) = await ReviewContentAsync(content);

            // If content is good enough, return it
            if (status.Contains("Good") || status.Contains("Excellent"))
            {
                return content;
            }

            // If not the last attempt, try to improve
            if (attempt < maxAttempts)
            {
                content = await ImproveContentAsync(content, notes);
            }
        }

        return content; // Return best attempt
    }

    // -----------------------------
    // Helper: Improve content based on review
    // -----------------------------
    private async Task<string> ImproveContentAsync(string originalContent, string reviewNotes)
    {
        string prompt = $@"Improve this paragraph based on the feedback provided:

Original Content:
{originalContent}

Feedback:
{reviewNotes}

Write an improved version that addresses the feedback:";

        var result = await _kernel.InvokePromptAsync(prompt);
        return result.GetValue<string>() ?? originalContent;
    }

    // -----------------------------
    // Helper: Extract status from review text
    // -----------------------------
    private string ExtractStatusFromReview(string reviewText)
    {
        var lowerReview = reviewText.ToLower();

        if (lowerReview.Contains("excellent"))
            return "Excellent";
        else if (lowerReview.Contains("good"))
            return "Good";
        else if (lowerReview.Contains("needs work") || lowerReview.Contains("poor"))
            return "Needs Work";
        else
            return "Good"; // Default assumption
    }

    // -----------------------------
    // Advanced: Function Calling with Semantic Kernel
    // -----------------------------
    public async Task<string> GenerateBlogWithFunctionAsync(string blogTitle)
    {
        // Create a function from the method
        var outlineFunction = _kernel.CreateFunctionFromMethod(
            method: GenerateOutlineAsync,
            functionName: "GenerateOutline",
            description: "Generate blog outline from title"
        );

        var contentFunction = _kernel.CreateFunctionFromMethod(
            method: GenerateContentBlockAsync,
            functionName: "GenerateContent",
            description: "Generate content block from outline item"
        );

        // Use the functions in a more complex workflow
        var arguments = new KernelArguments
        {
            ["blogTitle"] = blogTitle
        };

        var outline = await _kernel.InvokeAsync(outlineFunction, arguments);

        // Process the outline and generate content
        // This is a simplified example - you'd implement more complex logic here

        return $"Generated blog for: {blogTitle}";
    }
}