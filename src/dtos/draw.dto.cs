
public class DrawDto {
    
}

public class ElementDto
{
    public string Id { get; set; } = string.Empty;

    // rectangle, arrow, text, ellipse, line...
    public string Type { get; set; } = string.Empty;

    public double X { get; set; }
    public double Y { get; set; }

    public double Width { get; set; }
    public double Height { get; set; }

    public string StrokeColor { get; set; } = "#000000";
    public string BackgroundColor { get; set; } = "transparent";

    public int StrokeWidth { get; set; }

    public double Angle { get; set; }

    public string Text { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
}

public class CanvasDto
{
    public string Id { get; set; } = string.Empty;

    public List<ElementDto> Elements { get; set; } = new();

    public double Zoom { get; set; }

    public string BackgroundColor { get; set; } = "#ffffff";
}