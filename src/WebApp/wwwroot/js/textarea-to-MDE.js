const textareaList = document.querySelectorAll("textarea.EasyMDE");
textareaList.forEach((area) => {
  new EasyMDE({
    renderingConfig: {
      sanitizerFunction: (renderedHTML) => {
        return DOMPurify.sanitize(renderedHTML, { USE_PROFILES: { html: true } });
      },
    },
    element: area,
    placeholder: "Type here...",
    toolbar: [
      "bold",
      "italic",
      "|",
      "heading-1",
      "heading-2",
      "heading-3",
      "|",
      "quote",
      "unordered-list",
      "ordered-list",
      "table",
      "|",
      "preview",
      "side-by-side",
      "fullscreen",
      "guide",
    ],
    toolbarButtonClassPrefix: "mde",
  });
});
