using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mpa.Models
{
  public class Memo
  {
    private string UserID { get; set; }
    public int MemoID { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }
  }
}