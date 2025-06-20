import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { DashboardService } from '../../admin/dashboard/dashboard.service';

interface DashboardStats {
  availableRooms: number;
  reservations: number;
  pendingRequests: number;
  checkIns: number;
  checkOuts: number;
}

interface BedsStats {
  available: number;
  occupied: number;
  revenue: number;
}

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class AdminDashboardComponent implements OnInit {
  currentDate = new Date(); // Current date
  selectedDate = new Date(2025, 4, 13); // Default selected date (May 13, 2025)
  
  // Form group for date range selection
  range = new FormGroup({
    start: new FormControl<Date | null>(null),
    end: new FormControl<Date | null>(null),
  });

  // Dashboard metrics
  stats: DashboardStats = {
    availableRooms: 0,
    reservations: 0,
    pendingRequests: 0,
    checkIns: 0,
    checkOuts: 0,
  };

  // Bed statistics
  bedsStats: BedsStats = {
    available: 0,
    occupied: 0,
    revenue: 0,
  };

  // Calendar days for rendering
  calendarDays: Date[] = [];

  constructor(private dashboardService: DashboardService) {}

  ngOnInit() {
    this.generateCalendarDays(); // Generate calendar days
    this.setupDateRangeListener(); // Listen for date range changes
    this.fetchDashboardStats(); // Fetch dashboard stats
    this.fetchBedsStats(); // Fetch bed stats
  }

  calculatePrice(roomId: number, startDate: string, endDate: string): number {
  const pricePerDay = {
    1: 5000, // Deluxe Room
    2: 3000, // Standard Room
    3: 8000, // Suite
  }[roomId] || 0;

  const start = new Date(startDate);
  const end = new Date(endDate);
  const days = Math.ceil((end.getTime() - start.getTime()) / (1000 * 3600 * 24));
  return pricePerDay * days;
}

  /**
   * Sets up a listener for changes in the date range form
   */
  setupDateRangeListener() {
    this.range.valueChanges.subscribe((range) => {
      if (range.start && range.end) {
        this.fetchBedsStats(range.start, range.end);
      }
    });
  }

  fetchDashboardStats() {
    const today = this.currentDate.toISOString().split('T')[0]; // Format today as YYYY-MM-DD

    // Fetch all bookings
    this.dashboardService.getAllBookings().subscribe((bookings: any[]) => {
      this.stats.reservations = bookings.length;
      this.stats.checkIns = bookings.filter((b) => b.checkInDate === today).length;
      this.stats.checkOuts = bookings.filter((b) => b.checkOutDate === today).length;
      this.stats.pendingRequests = bookings.filter((b) => b.status === 'Pending').length;

      // Calculate total revenue from bookings
      this.bedsStats.revenue = bookings.reduce((sum, b) => {
      const calculatedPrice = this.calculatePrice(b.roomId, b.startDate, b.endDate);
      b.price = calculatedPrice; // Set the calculated price on the booking object
      return sum + calculatedPrice;
    }, 0);
    });

    // Fetch available beds for a specific room (roomId = 1 for demonstration)
    const todayDate = today; // Use today string for available beds
    this.dashboardService.getAvailableBeds(1, todayDate, todayDate).subscribe((data) => {
    this.stats.availableRooms = data.length;
    });
  }

  /**
   * @param startDate Start date of the range
   * @param endDate End date of the range
   */
  fetchBedsStats(startDate: Date = this.currentDate, endDate: Date = this.currentDate) {
    const start = startDate.toISOString().split('T')[0];
    const end = endDate.toISOString().split('T')[0];

    // Fetch available beds for a specific room (roomId = 1 for demonstration)
    this.dashboardService.getAvailableBeds(1, start, end).subscribe((data) => {
      this.bedsStats.available = data.length;

      // Assume total beds is a constant (62 for demonstration)
      const totalBeds = 62;
      this.bedsStats.occupied = totalBeds - this.bedsStats.available;
    });
  }

  generateCalendarDays() {
    const year = this.selectedDate.getFullYear();
    const month = this.selectedDate.getMonth();

    const firstDay = new Date(year, month, 1); // First day of the month
    const lastDay = new Date(year, month + 1, 0); // Last day of the month

    this.calendarDays = [];

    // Add days from the previous month to align the calendar view
    const daysFromPrevMonth = (firstDay.getDay() + 6) % 7;
    for (let i = daysFromPrevMonth; i > 0; i--) {
      this.calendarDays.push(new Date(year, month, -i + 1));
    }

    // Add days of the current month
    for (let i = 1; i <= lastDay.getDate(); i++) {
      this.calendarDays.push(new Date(year, month, i));
    }

    // Add days from the next month to complete the calendar view (6 rows of 7 days)
    const remainingDays = 42 - this.calendarDays.length;
    for (let i = 1; i <= remainingDays; i++) {
      this.calendarDays.push(new Date(year, month + 1, i));
    }
  }

  /**
   * Moves the calendar to the previous month
   */
  previousMonth() {
    this.selectedDate = new Date(
      this.selectedDate.getFullYear(),
      this.selectedDate.getMonth() - 1
    );
    this.generateCalendarDays();
  }

  /**
   * Moves the calendar to the next month
   */
  nextMonth() {
    this.selectedDate = new Date(
      this.selectedDate.getFullYear(),
      this.selectedDate.getMonth() + 1
    );
    this.generateCalendarDays();
  }

  /**
   * Refreshes the bed statistics based on the selected date range
   */
  refreshBedsStats() {
    const range = this.range.value;
    if (range.start && range.end) {
      this.fetchBedsStats(range.start, range.end);
    }
  }

  /**
   * Checks if a given date is today
   * 
   * @param date The date to check
   * @returns True if the date is today, false otherwise
   */
  isToday(date: Date): boolean {
    const today = new Date();
    return (
      date.getDate() === today.getDate() &&
      date.getMonth() === today.getMonth() &&
      date.getFullYear() === today.getFullYear()
    );
  }

  /**
   * Checks if a given date belongs to the currently selected month
   * 
   * @param date The date to check
   * @returns True if the date is in the current month, false otherwise
   */
  isCurrentMonth(date: Date): boolean {
    return date.getMonth() === this.selectedDate.getMonth();
  }
}