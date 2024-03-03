using System.ComponentModel.DataAnnotations;

namespace FileMqBroker.MqLibrary.Models;

/// <summary>
/// The state of the file that contains the message.
/// </summary>
public enum MessageFileState
{
    [Display(Name = "Undefined")]
    Undefined,

    [Display(Name = "Created")]
    Created,

    [Display(Name = "Reading")]
    Reading,

    [Display(Name = "Writing")]
    Writing,

    [Display(Name = "Deleting")]
    Deleting,

    [Display(Name = "Ready to Write")]
    ReadyToWrite,

    [Display(Name = "Ready to Read")]
    ReadyToRead,

    [Display(Name = "Ready to Delete")]
    ReadyToDelete,

    [Display(Name = "Failed to Write")]
    FailedToWrite,

    [Display(Name = "Failed to Read")]
    FailedToRead,

    [Display(Name = "Failed to Delete")]
    FailedToDelete,
    
    [Display(Name = "Processed")]
    Processed,
    
    [Display(Name = "Deleted")]
    Deleted
}