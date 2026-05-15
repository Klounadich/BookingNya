using BookingModule.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Shared.Enums;

namespace BookingModule.PDF;

public class BookingReceiptDocument : IDocument
{
    private readonly BookingModel _booking;
    
    public BookingReceiptDocument(BookingModel booking)
    {
        _booking = booking;
    }
    
    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;
    
    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(40);
            page.Size(PageSizes.A6);
            page.DefaultTextStyle(x => x.FontSize(10));
            
            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Element(ComposeFooter);
        });
    }
    
    private void ComposeHeader(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(10);
            
            column.Item().AlignCenter().Text("ЧЕК ОБ ОПЛАТЕ")
                .FontSize(18)
                .Bold()
                .FontColor(Colors.Blue.Darken2);
            
            column.Item().AlignCenter().Text("Отель Booking Pro")
                .FontSize(12)
                .FontColor(Colors.Grey.Darken2);
            
            column.Item().LineHorizontal(1);
        });
    }
    
    private void ComposeContent(IContainer container)
    {
        var nights = (_booking.check_out - _booking.check_in).Days;
        
        container.Column(column =>
        {
            column.Spacing(10);
            
            
            column.Item().Text("ДЕТАЛИ БРОНИРОВАНИЯ").Bold().FontSize(12);
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                });
                
                table.Cell().Text("ID брони:");
                table.Cell().Text(_booking.id.ToString());
                
                
                table.Cell().Text("Номер комнаты:");
                table.Cell().Text(_booking.room_id);
                
                table.Cell().Text("Статус:");
                table.Cell().Text(GetStatusText(_booking.status))
                    .FontColor(GetStatusColor(_booking.status));
            });
            
            column.Item().Text("ДАННЫЕ ГОСТЯ").Bold().FontSize(12);
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                });
                
                table.Cell().Text("Имя:");
                table.Cell().Text(_booking.guest_name);
                
                table.Cell().Text("Email:");
                table.Cell().Text(_booking.guest_email);
                
                table.Cell().Text("Телефон:");
                table.Cell().Text(_booking.guest_phone);
            });
            column.Item().Text("ДЕТАЛИ ПРОЖИВАНИЯ").Bold().FontSize(12);
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                });
                
                table.Cell().Text("Заезд:");
                table.Cell().Text(_booking.check_in.ToString("dd.MM.yyyy") + " (14:00)");
                
                table.Cell().Text("Выезд:");
                table.Cell().Text(_booking.check_out.ToString("dd.MM.yyyy") + " (12:00)");
                
                table.Cell().Text("Количество ночей:");
                table.Cell().Text($"{nights} ноч(ей)");
            });
            
            column.Item().Text("ФИНАНСОВАЯ ИНФОРМАЦИЯ").Bold().FontSize(12);
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                });
                
                table.Cell().Text("Сумма к оплате:");
                table.Cell().Text($"{_booking.total_price:F2} {_booking.currency}")
                    .Bold()
                    .FontColor(Colors.Green.Darken2); 
                
                table.Cell().Text("Метод оплаты:");
                table.Cell().Text(string.IsNullOrEmpty(_booking.payment_method) ? "Не указан" : _booking.payment_method);
                
                if (!string.IsNullOrEmpty(_booking.payment_reservation_id))
                {
                    table.Cell().Text("ID платежа:");
                    table.Cell().Text(_booking.payment_reservation_id);
                }
                
                table.Cell().Text("Дата создания:");
                table.Cell().Text(_booking.created_at.ToString("dd.MM.yyyy HH:mm"));
            });
            
            
            if (_booking.status == BookingStatus.Cancelled)
            {
                column.Item().Text("ИНФОРМАЦИЯ ОБ ОТМЕНЕ").Bold().FontSize(12);
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(2);
                    });
                    
                    table.Cell().Text("Дата отмены:");
                    table.Cell().Text(_booking.cancelled_at?.ToString("dd.MM.yyyy HH:mm") ?? "Не указана");
                    
                    if (!string.IsNullOrEmpty(_booking.cancellation_reason))
                    {
                        table.Cell().Text("Причина:");
                        table.Cell().Text(_booking.cancellation_reason);
                    }
                });
            }
            
            column.Item().LineHorizontal(1);
            
            column.Item().AlignCenter().Text("Спасибо за выбор нашего отеля!")
                .FontSize(11)
                .Bold()
                .FontColor(Colors.Blue.Darken1);
        });
    }
    
    private void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Text(text =>
        {
            text.Span("Чек сгенерирован автоматически. ");
            text.Span(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")).FontSize(8);
        });
    }
    
    private string GetStatusText(BookingStatus status)
    {
        return status switch
        {
            BookingStatus.Pending => "Ожидание",
            BookingStatus.Confirmed => "Подтверждено",
            BookingStatus.Cancelled => "Отменено",
            BookingStatus.Completed => "Завершено",
            _ => status.ToString()
        };
    }
    
    private string GetStatusColor(BookingStatus status)
    {
        return status switch
        {
            BookingStatus.Pending => Colors.Orange.Medium,    
            BookingStatus.Confirmed => Colors.Green.Medium,  
            BookingStatus.Cancelled => Colors.Red.Medium,    
            BookingStatus.Completed => Colors.Blue.Medium,   
            _ => Colors.Black
        };
    }
}