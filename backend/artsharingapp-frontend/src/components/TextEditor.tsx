import { useEditor, EditorContent, BubbleMenu } from "@tiptap/react";
import StarterKit from "@tiptap/starter-kit";
import "../styles/TextEditor.css";
import Placeholder from "@tiptap/extension-placeholder";
import TextStyle from "@tiptap/extension-text-style";
import Color from "@tiptap/extension-color";
import { LuHeading1, LuHeading2 } from "react-icons/lu";
import { FiBold, FiItalic } from "react-icons/fi";
import { TbLetterT } from "react-icons/tb";
import { IoIosColorPalette, IoIosList } from "react-icons/io";
import { useEffect, useMemo } from "react";
import { Editor } from "@tiptap/react";

type TextEditorProps = {
  content?: string;
  editable?: boolean;
  className?: string;
  onUpdate?: (props: { editor: Editor }) => void;
  disableHeadings?: boolean;
};

const TextEditor = ({
  content = "",
  editable = true,
  className = "",
  onUpdate,
  disableHeadings = false,
}: TextEditorProps) => {
  const extensions = useMemo(
    () => [
      StarterKit.configure({
        heading: disableHeadings ? false : { levels: [1, 2] },
      }),
      Placeholder.configure({
        placeholder: "Start typing here...",
      }),
      TextStyle,
      Color,
    ],
    [disableHeadings]
  );

  const editor = useEditor({
    extensions,
    content,
    editable: editable,
    editorProps: {
      attributes: {
        class: className,
      },
    },
    onUpdate,
  });

  useEffect(() => {
    if (editor) {
      editor.setEditable(editable);
    }
  }, [editable]);

  useEffect(() => {
    if (editor && content !== editor.getHTML()) {
      editor.commands.setContent(content || "", false);
    }
  }, [content, editor]);

  if (!editor) {
    return null;
  }

  return (
    <div>
      <EditorContent editor={editor} />
      <BubbleMenu editor={editor}>
        <div className="bubble-menu">
          {!disableHeadings && (
            <>
              <button
                onClick={() => {
                  editor.chain().focus().unsetAllMarks().run();
                  editor.commands.toggleHeading({ level: 1 });
                }}
                className={
                  editor.isActive("heading", { level: 1 }) ? "is-active" : ""
                }
              >
                <LuHeading1 />
              </button>
              <button
                onClick={() => {
                  editor.chain().focus().unsetAllMarks().run();
                  editor.commands.toggleHeading({ level: 2 });
                }}
                className={
                  editor.isActive("heading", { level: 2 }) ? "is-active" : ""
                }
              >
                <LuHeading2 />
              </button>
            </>
          )}
          <button
            onClick={() => {
              editor.chain().focus().unsetAllMarks().run();
              editor.commands.toggleBulletList();
            }}
            className={editor.isActive("bulletList") ? "is-active" : ""}
          >
            <IoIosList />
          </button>

          <button
            onClick={() => editor.chain().focus().toggleBold().run()}
            className={editor.isActive("bold") ? "is-active" : ""}
          >
            <FiBold />
          </button>
          <button
            onClick={() => editor.chain().focus().toggleItalic().run()}
            className={editor.isActive("italic") ? "is-active" : ""}
          >
            <FiItalic />
          </button>

          <button
            onClick={() => {
              editor.commands.setParagraph();
              editor.chain().focus().unsetColor().run();
              editor.chain().focus().unsetAllMarks().run();
            }}
          >
            <TbLetterT />
          </button>

          <button
            onClick={() => editor.chain().focus().setColor("#958DF1").run()}
            className={
              editor.isActive("textStyle", { color: "#958DF1" })
                ? "is-active"
                : ""
            }
            data-testid="setPurple"
          >
            <IoIosColorPalette />
          </button>
        </div>
      </BubbleMenu>
    </div>
  );
};

export default TextEditor;
