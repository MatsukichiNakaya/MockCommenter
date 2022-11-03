using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MockCommenter
{
    public class CommentStyleSelector : DataTemplateSelector
    {
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
        public DataTemplate Template1 { get; set; }
        public DataTemplate Template2 { get; set; }
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
        public override DataTemplate SelectTemplate(Object item, DependencyObject container)
        {
            var info = item as CommentInfo;

            if ((info?.Pay ?? 0) <= 0) {
				return this.Template1;
			} else {
				return this.Template2;
			}
		}
    }
}
